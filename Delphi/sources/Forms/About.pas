unit About;

interface

uses
  System.SysUtils, System.Types, System.UITypes, System.Classes, System.Variants,
  FMX.Types, FMX.Controls, FMX.Forms, FMX.Graphics, FMX.Dialogs, FMX.StdCtrls,
  FMX.Controls.Presentation, FMX.ScrollBox, FMX.Memo;

type
  TfmAbout = class(TForm)
    tbHeader: TToolBar;
    tbFooter: TToolBar;
    lbTitle: TLabel;
    btOk: TSpeedButton;
    mmAbout: TMemo;
    procedure btOkClick(Sender: TObject);
  end;

var
  fmAbout: TfmAbout;

implementation

{$R *.fmx}

procedure TfmAbout.btOkClick(Sender: TObject);
begin
  Close;
end;

end.
