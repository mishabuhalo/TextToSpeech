unit reSpeech;

interface

uses
  System.Classes, System.SysUtils, System.DateUtils, System.StrUtils,
  System.IOUtils, System.Types, System.JSON, System.Generics.Collections,
  FMX.Forms, BASS, FMX.Platform.Win;

type
  TPart = class
    Part: string;
    Start: integer;
    Duration: integer;
  end;
  TParts = TObjectList<TPart>;

type
  TreSpeechThread = class(TThread)
  private
    function GetStream: TStream;
  protected
    FStream: TMemoryStream;
    FParts: TParts;
    FChannel: HSTREAM;
    FText: string;
    FPosition: integer;
    procedure GetPart(var AIndex: integer; var ALength: integer);
    procedure PlayPart(AIndex: integer);
    procedure Execute; override;
    procedure SetText(const AText: string);
    procedure SetPosition(APosition: integer);
    procedure SetStream(AStream: TStream);
  public
    constructor Create(CreateSuspended: Boolean);
    destructor Destroy; override;
    property Text: string read FText write SetText;
    property Position: integer read FPosition write SetPosition;
    property Stream: TStream write SetStream;
    property Parts: TParts read FParts write FParts;
  end;

  TreSpeech = class(TComponent)
  protected
    FThread: TreSpeechThread;
    FStream: TResourceStream;
    FParts: TParts;
    FText: string;
    FPosition: integer;
    FActive: boolean;
    FOnStart: TNotifyEvent;
    FOnStop: TNotifyEvent;
    procedure SetText(const AText: string);
    procedure SetPosition(APosition: integer);
    procedure DoStop(Sender: TObject);
    procedure InitParts;
    function StreamToString(AStream: TStream): string;
  public
    constructor Create(AOwner: TComponent); override;
    destructor Destroy; override;
    procedure Start;
    procedure Stop;
  published
    property Active: boolean read FActive;
    property Text: string read FText write SetText;
    property Position: integer read FPosition write SetPosition;
    property OnStart: TNotifyEvent read FOnStart write FOnStart;
    property OnStop: TNotifyEvent read FOnStop write FOnStop;
  end;

procedure Register;

implementation

procedure Register;
begin
  RegisterComponents('re0ne', [TreSpeech]);
end;

function Now: int64;
begin
  Result := System.DateUtils.MilliSecondsBetween(System.SysUtils.Now, 0);
end;

{ TreSpeech }

constructor TreSpeech.Create(AOwner: TComponent);
begin
  inherited;
  FThread := nil;
  FActive := false;
  FText := '';
  FPosition := 0;
  FStream := TResourceStream.Create(HInstance, 'SoundsWav', RT_RCDATA);
  FParts := TParts.Create(True);
  InitParts;
end;

destructor TreSpeech.Destroy;
begin
  FreeAndNil(FParts);
  FreeAndNil(FStream);
  inherited;
end;

procedure TreSpeech.DoStop(Sender: TObject);
begin
  FActive := false;
  FPosition := FThread.Position;
  FThread := nil;

  if Assigned(FOnStop) then
    FOnStop(Self);
end;

procedure TreSpeech.InitParts;
var
  AStream: TResourceStream;
  Data: TJSONArray;
  Item: TJSONObject;
  Part: TPart;
  i: integer;
begin
  AStream := TResourceStream.Create(HInstance, 'SoundsJson', RT_RCDATA);
  try
    Data := TJSONObject.ParseJSONValue(StreamToString(AStream)) as TJSONArray;
    if Assigned(Data) then begin
      for i := 0 to Data.Count - 1 do begin
        Item := Data.Items[i] as TJSONObject;
        Part := TPart.Create;
        Part.Part := (Item.Values['Part'] as TJSONString).Value;
        Part.Start := (Item.Values['Start'] as TJSONNumber).AsInt;
        Part.Duration := (Item.Values['Duration'] as TJSONNumber).AsInt;
        FParts.Add(Part);
      end;
    end;
  finally
    FreeAndNil(AStream);
  end;
end;

procedure TreSpeech.SetPosition(APosition: integer);
begin
  if (APosition <= 0) or (APosition > FText.Length)
    then FPosition := 1
    else FPosition := APosition;
end;

procedure TreSpeech.SetText(const AText: string);
begin
  FText := AText;
  FPosition := 1;
end;

procedure TreSpeech.Start;
begin
  FActive := true;

  if Assigned(FOnStart) then
    FOnStart(Self);

  FThread := TreSpeechThread.Create(True);
  FThread.OnTerminate := DoStop;
  FThread.Stream := FStream;
  FThread.Parts := FParts;
  FThread.Text := FText;
  FThread.Position := FPosition;
  FThread.Start;
end;

procedure TreSpeech.Stop;
begin
  if Assigned(FThread) then
    FThread.Terminate;
end;

function TreSpeech.StreamToString(AStream: TStream): string;
var
  AStrings: TStringList;
begin
  AStrings := TStringList.Create;
  try
    AStrings.LoadFromStream(AStream);
    Result := UTF8ToString(AStrings.Text);
  finally
    FreeAndNil(AStrings);
  end;
end;

{ TreSpeechThread }

constructor TreSpeechThread.Create(CreateSuspended: Boolean);
begin
  inherited;
  FreeOnTerminate := true;
  FStream := TMemoryStream.Create;
  BASS_Init(-1, 44100, BASS_DEVICE_DEFAULT, FMX.Platform.Win.WindowHandleToPlatform(Application.MainForm.Handle).Wnd, nil);
  Bass.BASS_SetVolume(0.8);
end;

destructor TreSpeechThread.Destroy;
begin
  if FChannel <> 0 then
    BASS_StreamFree(FChannel);
  BASS_Free;
  FreeAndNil(FStream);
  inherited;
end;

procedure TreSpeechThread.Execute;
var
  Index, Length: integer;
begin
  while (not Terminated) and (FPosition <= FText.Length) do begin
    GetPart(Index, Length);
    PlayPart(Index);
    inc(FPosition, Length);
  end;
  BASS_ChannelStop(FChannel);

  Sleep(100);
end;

procedure TreSpeechThread.GetPart(var AIndex, ALength: integer);
var
  Part: string;
  Position: integer;
  i: integer;
begin
  AIndex := -1;
  ALength := 1;

  for i := 0 to FParts.Count - 1 do begin
    Part := Parts[i].Part;
    Position := PosEx(Part, FText, FPosition);

    if (Position > 0) and (Position = FPosition) then begin
      AIndex := i;
      ALength := Part.Length;

      break;
    end;
  end;
end;

function TreSpeechThread.GetStream: TStream;
begin
  Result := FStream;
end;

procedure TreSpeechThread.PlayPart(AIndex: integer);
var
  StartFrom: int64;
  Duration: integer;
  Part: TPart;
begin
  if (AIndex >= 0) and (AIndex < FParts.Count) then begin
    Part := Parts[AIndex];
    StartFrom := Part.Start;
    Duration := Part.Duration;

    if StartFrom >= 0 then begin
      BASS_ChannelSetPosition(FChannel,
        BASS_ChannelSeconds2Bytes(FChannel, StartFrom / 1000.0), BASS_POS_BYTE);
      Bass.BASS_ChannelPlay(FChannel, False);
      Sleep(Duration);
      BASS_ChannelPause(FChannel);
    end else
      Sleep(Duration);
  end;
end;

procedure TreSpeechThread.SetPosition(APosition: integer);
begin
  if (APosition <= 0) or (APosition > FText.Length)
    then FPosition := 1
    else FPosition := APosition;
end;

procedure TreSpeechThread.SetStream(AStream: TStream);
begin
  AStream.Position := 0;
  FStream.CopyFrom(AStream, AStream.Size);
  if FChannel <> 0 then
    BASS_StreamFree(FChannel);
  FChannel := BASS_StreamCreateFile(True, FStream.Memory, 0, FStream.Size, 0);
end;

procedure TreSpeechThread.SetText(const AText: string);
begin
  FText := AnsiLowerCase(AText);
  FPosition := 1;
end;

end.
