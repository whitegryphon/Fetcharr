; Script generated by the Inno Setup Script Wizard.
; SEE THE DOCUMENTATION FOR DETAILS ON CREATING INNO SETUP SCRIPT FILES!

#define AppName "Fetcharr"
#define AppPublisher "Team Fetcharr"
#define AppURL "https://fetcharr.com/"
#define ForumsURL "https://fetcharr.com/discord/"
#define AppExeName "Fetcharr.exe"
#define BaseVersion GetEnv('MAJORVERSION')
#define BuildNumber GetEnv('MINORVERSION')
#define BuildVersion GetEnv('FETCHARRVERSION')

[Setup]
; NOTE: The value of AppId uniquely identifies this application.
; Do not use the same AppId value in installers for other applications.
; (To generate a new GUID, click Tools | Generate GUID inside the IDE.)
AppId={{2B42F178-4D5F-472A-8D31-EC08376F01B4}
AppName={#AppName}
AppVersion={#BaseVersion}
AppPublisher={#AppPublisher}
AppPublisherURL={#AppURL}
AppSupportURL={#ForumsURL}
AppUpdatesURL={#AppURL}
DefaultDirName={commonappdata}\Fetcharr
DisableDirPage=yes
DefaultGroupName={#AppName}
DisableProgramGroupPage=yes
OutputBaseFilename=Fetcharr.{#BuildVersion}.{#Runtime}
SolidCompression=yes
AppCopyright=Creative Commons 3.0 License
AllowUNCPath=False
UninstallDisplayIcon={app}\bin\Fetcharr.exe
DisableReadyPage=True
CompressionThreads=2
Compression=lzma2/normal
AppContact={#ForumsURL}
VersionInfoVersion={#BaseVersion}.{#BuildNumber}
SetupLogging=yes
OutputDir=output
WizardStyle=modern

[Languages]
Name: "english"; MessagesFile: "compiler:Default.isl"

[Tasks]
Name: "desktopIcon"; Description: "{cm:CreateDesktopIcon}"
Name: "windowsService"; Description: "Install Windows Service (Starts when the computer starts as the LocalService user, you will need to change the user to access network shares)"; GroupDescription: "Start automatically"; Flags: exclusive
Name: "startupShortcut"; Description: "Create shortcut in Startup folder (Starts when you log into Windows)"; GroupDescription: "Start automatically"; Flags: exclusive unchecked
Name: "none"; Description: "Do not start automatically"; GroupDescription: "Start automatically"; Flags: exclusive unchecked

[Dirs]
Name: "{app}"; Permissions: users-modify

[Files]
Source: "..\..\..\_artifacts\{#Runtime}\{#Framework}\Fetcharr\Fetcharr.exe"; DestDir: "{app}\bin"; Flags: ignoreversion
Source: "..\..\..\_artifacts\{#Runtime}\{#Framework}\Fetcharr\*"; Excludes: "Fetcharr.Update"; DestDir: "{app}\bin"; Flags: ignoreversion recursesubdirs createallsubdirs
; NOTE: Don't use "Flags: ignoreversion" on any shared system files

[Icons]
Name: "{group}\{#AppName}"; Filename: "{app}\bin\{#AppExeName}"; Parameters: "/icon"
Name: "{commondesktop}\{#AppName}"; Filename: "{app}\bin\{#AppExeName}"; Parameters: "/icon"; Tasks: desktopIcon
Name: "{userstartup}\{#AppName}"; Filename: "{app}\bin\Fetcharr.exe"; WorkingDir: "{app}\bin"; Tasks: startupShortcut

[InstallDelete]
Name: "{app}\bin"; Type: filesandordirs

[Run]
Filename: "{app}\bin\Fetcharr.Console.exe"; StatusMsg: "Removing previous Windows Service"; Parameters: "/u /exitimmediately"; Flags: runhidden waituntilterminated;
Filename: "{app}\bin\Fetcharr.Console.exe"; Description: "Enable Access from Other Devices"; StatusMsg: "Enabling Remote access"; Parameters: "/registerurl /exitimmediately"; Flags: postinstall runascurrentuser runhidden waituntilterminated; Tasks: startupShortcut none;
Filename: "{app}\bin\Fetcharr.Console.exe"; StatusMsg: "Installing Windows Service"; Parameters: "/i /exitimmediately"; Flags: runhidden waituntilterminated; Tasks: windowsService
Filename: "{app}\bin\Fetcharr.exe"; Description: "Open Fetcharr Web UI"; Flags: postinstall skipifsilent nowait; Tasks: windowsService;
Filename: "{app}\bin\Fetcharr.exe"; Description: "Start Fetcharr"; Flags: postinstall skipifsilent nowait; Tasks: startupShortcut none;

[UninstallRun]
Filename: "{app}\bin\fetcharr.console.exe"; Parameters: "/u"; Flags: waituntilterminated skipifdoesntexist

[Code]
function PrepareToInstall(var NeedsRestart: Boolean): String;
var
  ResultCode: Integer;
begin
  Exec('net', 'stop fetcharr', '', 0, ewWaitUntilTerminated, ResultCode)
  Exec('sc', 'delete fetcharr', '', 0, ewWaitUntilTerminated, ResultCode)
end;
