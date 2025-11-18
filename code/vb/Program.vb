'DOC
Imports Sinumerik.Advanced

'DOC
Dim client = New SinumerikDevice("192.168.0.131")

'DOC
client.Connect();

'DOC
' Your code to interact with the controller.

'DOC
client.Disconnect()

'DOC
Using var client = new SinumerikClient("s840d.sl://192.168.0.131"))
    client.Connect()
    ' Your code to interact with the controller.
End Using

//DOC
' Read machine axis position
client.ReadValue("/Channel/MachineAxis/measPos1[u1, 1]")
client.ReadValue("/Channel/MachineAxis/measPos1")

' Read using token format
client.ReadValue("2 1 5 1 71")

' Read array element
client.ReadValue("MESS_PA1[1, 1]")

' Read R-Parameter
client.ReadValue("/Channel/Parameter/R[41]")

' Read GUD variable (requires .DEF file to be loaded first)
client.ReadValue("/GUD/GUD_VARIABLE_NAME")

//DOC
' ReadValues with single address returning multiple values
Dim dcValues = client.ReadValues("/DriveVsa/DC/R0026")
Console.WriteLine($"DC voltage: {dcValues(0)}")

' Reading multiple axis positions from array
Dim positions = client.ReadValues("/Channel/MachineAxis/measPos1")
For i As Integer = 0 To positions.Length - 1
    Console.WriteLine($"Axis {i}: {positions(i)}")
Next

' ReadValues with multiple different addresses
Dim values = client.ReadValues(
    "/DriveVsa/DC/R0026",
    "/Channel/MachineAxis/measPos1[u1, 1]",
    "/Channel/Parameter/R[41]"
)
Console.WriteLine($"DC voltage: {values(0)}")
Console.WriteLine($"Position: {values(1)}")
Console.WriteLine($"R41: {values(2)}")


//DOC
' Write single value to machine axis position
client.WriteValue("/Channel/MachineAxis/measPos1[u1, 1]", 51966)

' Write R-Parameter
client.WriteValue("/Channel/Parameter/R[41]", 123.456)

' Write GUD variable
client.WriteValue("/GUD/MY_VARIABLE", 42)

//DOC
' Load GUD definition files
client.LoadVariables("C:\GUD\DEFINITIONS.DEF")

' After loading, GUD variables can be accessed
Dim gudValue = client.ReadValue("/GUD/MY_VARIABLE")
Console.WriteLine($"GUD Variable: {gudValue}")

//DOC
' Get root directory
Dim wks = client.GetDirectory("/_N_WKS_DIR")

' Get subdirectories
Dim dirs = wks.GetDirectories()
For Each dir In dirs
    Console.WriteLine($"Directory: {dir.Name}")
Next

' Get files in directory
Dim files = wks.GetFiles()
For Each file In files
    Console.WriteLine($"File: {file.Name} ({file.Length} bytes)")
Next



//DOC
' Create new directory
client.CreateDirectory("/_N_WKS_DIR", "_N_NEW_MPF")

' Delete directory
client.DeleteDirectory("/_N_WKS_DIR", "_N_NEW_MPF")

//DOC
' Upload (read) file from controller
Dim ncFile As NcFile = client.Upload("/_N_WKS_DIR/_N_PROGRAM_FILE")
Dim fileContent = System.Text.Encoding.UTF8.GetString(ncFile.Content.ToArray())
Console.WriteLine($"File content: {fileContent}")

' Download (write) file to controller
Dim programContent = System.IO.File.ReadAllText("C:\Programs\MyProgram.mpf")
Dim ncFileToDownload = NcFile.Create(
    "/_N_WKS_DIR/PROGRAMS", 
    New ReadOnlyMemory(Of Byte)(System.Text.Encoding.UTF8.GetBytes(programContent))
)

client.Download("MY_PROGRAM", ncFileToDownload)

//DOC
Using client = New SinumerikClient("s840d.sl://192.168.0.131")
    client.Connect()
    client.SubscribeEvents(AddressOf HandleEvents)

    Console.WriteLine("Subscribed!")
    Console.ReadLine()
End Using

Private Sub HandleEvents(sender As Object, e As NcuEventReceivedEventArgs)
    Console.WriteLine(
        "{0}: ({1}) {2} ({3})",
        e.Event.Area,
        e.Event.EventId,
        e.Event.Message,
        If(e.Event.Timestamp?.ToString(), "----"))
End Sub



//DOC
' Load messages from CSV file
' CSV format: EventId (hex with "0x"); Message text
' Example: 0x220000;Custom alarm message
client.LoadMessages("C:\Messages\custom_messages.csv")

' Load messages from COM file (MBDDE format)
' COM files are typically stored on HMI at "$\dh\mb.dir\"
Dim messageProvider = NcuEventMessageProvider.Load("C:\Messages\mbdde_de.com")
NcuEvent.MessageProvider = messageProvider

//DOC
' Access default tokens
Dim defaultFile = NskFile.Default

For Each link In defaultFile.Links
    If link.Token Is Nothing OrElse link.ProtocolHandler <> 200 Then
        Continue For
    End If

    Dim value = link.CreateValue()
    Dim address As String = value.Type.Address.ToString()
    
    Console.WriteLine($"Name: {link.Name}, Address: {address}")
Next

//DOC
SinumerikNet.Advanced.Licenser.LicenseKey = "<insert your license code here>"

//DOC
Try
    Dim client = New SinumerikClient("s840d.sl://192.168.0.131")
    client.Connect()
    
    Dim value = client.ReadValue("/Channel/MachineAxis/measPos1[u1, 1]")
    Console.WriteLine($"Position: {value}")
    
    client.Disconnect()
Catch ex As NcuException
    Console.WriteLine($"NCU Error: {ex.Message}")
    Console.WriteLine($"Error Code: {ex.ErrorCode}")
Catch ex As NckException
    Console.WriteLine($"NCK Error: {ex.Message}")
    Console.WriteLine($"Reason: {ex.Reason}")
Catch ex As Exception
    Console.WriteLine($"General Error: {ex.Message}")
End Try








//DOC
