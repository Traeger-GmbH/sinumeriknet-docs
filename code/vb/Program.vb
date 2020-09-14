'DOC
Imports Sinumerik.Advanced

'DOC
Dim device = New SinumerikDevice("192.168.0.80")

'DOC
Dim connection = device.CreateConnection()
connection.Open()

'DOC
' Your code to interact with the controller.

'DOC
connection.Close()

'DOC
Using connection = device.CreateConnection()
    connection.Open()
    ' Your code to interact with the controller.
End Using
