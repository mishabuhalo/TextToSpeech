unit Main;

interface

uses
  System.SysUtils, System.Types, System.UITypes, System.Classes, System.Variants,
  FMX.Types, FMX.Controls, FMX.Forms, FMX.Graphics, FMX.Dialogs,
  FMX.Controls.Presentation, FMX.StdCtrls, FMX.ScrollBox, FMX.Memo,
  reSpeech;

type
  TfmMain = class(TForm)
    mmText: TMemo;
    tbHeader: TToolBar;
    tbFooter: TToolBar;
    btStartStop: TSpeedButton;
    lbTitle: TLabel;
    btAbout: TSpeedButton;
    procedure FormCreate(Sender: TObject);
    procedure FormDestroy(Sender: TObject);
    procedure btStartStopClick(Sender: TObject);
    procedure FormClose(Sender: TObject; var Action: TCloseAction);
    procedure btAboutClick(Sender: TObject);
  protected
    TTS: TreSpeech;
    procedure TTSStarted(Sender: TObject);
    procedure TTSStopped(Sender: TObject);
  end;

var
  fmMain: TfmMain;

implementation

{$R *.fmx}

uses
  About;

procedure TfmMain.btAboutClick(Sender: TObject);
begin
  fmAbout.Show;
end;

procedure TfmMain.btStartStopClick(Sender: TObject);
begin
  if TTS.Active then
    TTS.Stop
  else begin
    TTS.Text := mmText.Text;
    TTS.Start;
  end;
end;

procedure TfmMain.FormClose(Sender: TObject; var Action: TCloseAction);
begin
  TTS.Stop;
  Close;
end;

procedure TfmMain.FormCreate(Sender: TObject);
begin
  TTS := TreSpeech.Create(Self);
  TTS.OnStart := TTSStarted;
  TTS.OnStop := TTSStopped;
end;

procedure TfmMain.FormDestroy(Sender: TObject);
begin
  FreeAndNil(TTS);
end;

procedure TfmMain.TTSStarted(Sender: TObject);
begin
  btStartStop.Text := 'Зупинити';
end;

procedure TfmMain.TTSStopped(Sender: TObject);
begin
  btStartStop.Text := 'Читати';
end;

end.
