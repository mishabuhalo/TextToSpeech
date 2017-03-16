program TextToSpeech;





{$R *.dres}

uses
  System.StartUpCopy,
  FMX.Forms,
  Main in 'Forms\Main.pas' {fmMain},
  reSpeech in 'Speech\reSpeech.pas',
  About in 'Forms\About.pas' {fmAbout},
  bass in 'BASS\bass.pas';

{$R *.res}

begin
  Application.Initialize;
  Application.CreateForm(TfmMain, fmMain);
  Application.CreateForm(TfmAbout, fmAbout);
  Application.Run;
end.
