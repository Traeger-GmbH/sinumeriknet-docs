// DOC
using Sinumerik.Advanced;

// DOC
client = new SinumerikClient("s840d.sl://192.168.0.131");

// DOC
client.Connect();

// DOC
// Your code to interact with the controller.

// DOC
client.Disconnect();

// DOC
using (var client = new SinumerikClient("s840d.sl://192.168.0.131")) {
    client.Connect();    
    // Your code to interact with the controller.
}

//DOC
// Read machine axis position
client.ReadValue("/Channel/MachineAxis/measPos1[u1, 1]");
client.ReadValue("/Channel/MachineAxis/measPos1");

// Read using token format
client.ReadValue("2 1 5 1 71");

// Read array element
client.ReadValue("MESS_PA1[1, 1]");

// Read R-Parameter
client.ReadValue("/Channel/Parameter/R[41]");

// Read GUD variable (requires .DEF file to be loaded first)
client.ReadValue("/GUD/GUD_VARIABLE_NAME");

//DOC
// ReadValues with single address returning multiple values
var dcValues = client.ReadValues("/DriveVsa/DC/R0026");
Console.WriteLine($"DC voltage: {dcValues[0]}");

// Reading multiple axis positions from array
var positions = client.ReadValues("/Channel/MachineAxis/measPos1");
for (int i = 0; i < positions.Length; i++)
{
    Console.WriteLine($"Axis {i}: {positions[i]}");
}

// ReadValues with multiple different addresses
var values = client.ReadValues(
    "/DriveVsa/DC/R0026",
    "/Channel/MachineAxis/measPos1[u1, 1]",
    "/Channel/Parameter/R[41]"
);
Console.WriteLine($"DC voltage: {values[0]}");
Console.WriteLine($"Position: {values[1]}");
Console.WriteLine($"R41: {values[2]}");

//DOC
// Write single value to machine axis position
client.WriteValue("/Channel/MachineAxis/measPos1[u1, 1]", 51966);

// Write R-Parameter
client.WriteValue("/Channel/Parameter/R[41]", 123.456);

// Write GUD variable
client.WriteValue("/GUD/MY_VARIABLE", 42);

//DOC
// Load GUD definition files
client.LoadVariables(@"C:\GUD\DEFINITIONS.DEF");

// After loading, GUD variables can be accessed
var gudValue = client.ReadValue("/GUD/MY_VARIABLE");
Console.WriteLine($"GUD Variable: {gudValue}");

//DOC
// Get root directory
var wks = client.GetDirectory("/_N_WKS_DIR");

// Get subdirectories
var dirs = wks.GetDirectories();
foreach (var dir in dirs)
{
    Console.WriteLine($"Directory: {dir.Name}");
}

// Get files in directory
var files = wks.GetFiles();
foreach (var file in files)
{
    Console.WriteLine($"File: {file.Name} ({file.Length} bytes)");
}

//DOC
// Create new directory
client.CreateDirectory("/_N_WKS_DIR", "_N_NEW_MPF");

// Delete directory
client.DeleteDirectory("/_N_WKS_DIR", "_N_NEW_MPF");

//DOC
// Upload (read) file from controller
NcFile ncFile = client.Upload("/_N_WKS_DIR/_N_PROGRAM_FILE");
string fileContent = System.Text.Encoding.UTF8.GetString(ncFile.Content.ToArray());
Console.WriteLine($"File content: {fileContent}");

// Download (write) file to controller
string programContent = System.IO.File.ReadAllText(@"C:\Programs\MyProgram.mpf");
var ncFileToDownload = NcFile.Create(
    "/_N_WKS_DIR/PROGRAMS", 
    new ReadOnlyMemory<byte>(System.Text.Encoding.UTF8.GetBytes(programContent))
);

client.Download("MY_PROGRAM", ncFileToDownload);

//DOC
using (var client = new SinumerikClient("s840d.sl://192.168.0.131"))
{
    client.Connect();
    client.SubscribeEvents(HandleEvents);

    Console.WriteLine("Subscribed!");
    Console.ReadLine();
}

private static void HandleEvents(object sender, NcuEventReceivedEventArgs e)
{
    Console.WriteLine(
        "{0}: ({1}) {2} ({3})",
        e.Event.Area,
        e.Event.EventId,
        e.Event.Message,
        e.Event.Timestamp?.ToString() ?? "----");
}

//DOC
// Load messages from CSV file
// CSV format: EventId (hex with "0x"); Message text
// Example: 0x220000;Custom alarm message
client.LoadMessages(@"C:\Messages\custom_messages.csv");

// Load messages from COM file (MBDDE format)
// COM files are typically stored on HMI at "$\dh\mb.dir\"
var messageProvider = NcuEventMessageProvider.Load(@"C:\Messages\mbdde_de.com");
NcuEvent.MessageProvider = messageProvider;

//DOC
// Access default tokens
var defaultFile = NskFile.Default;

foreach (var link in defaultFile.Links)
{
    if (link.Token == null || link.ProtocolHandler != 200)
        continue;

    var value = link.CreateValue();
    string address = value.Type.Address.ToString();
    
    Console.WriteLine($"Name: {link.Name}, Address: {address}");
}

//DOC
SinumerikNet.Advanced.Licenser.LicenseKey = "<insert your license code here>";

//DOC
try
{
    var client = new SinumerikClient("s840d.sl://192.168.0.131");
    client.Connect();
    
    var value = client.ReadValue("/Channel/MachineAxis/measPos1[u1, 1]");
    Console.WriteLine($"Position: {value}");
    
    client.Disconnect();
}
catch (NcuException ex)
{
    Console.WriteLine($"NCU Error: {ex.Message}");
    Console.WriteLine($"Error Code: {ex.ErrorCode}");
}
catch (NckException ex)
{
    Console.WriteLine($"NCK Error: {ex.Message}");
    Console.WriteLine($"Reason: {ex.Reason}");
}
catch (Exception ex)
{
    Console.WriteLine($"General Error: {ex.Message}");
}

//DOC
