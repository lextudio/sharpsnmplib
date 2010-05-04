'
' Created by SharpDevelop.
' User: Administrator
' Date: 2010/4/25
' Time: 14:16
'
' To change this template use Tools | Options | Coding | Edit Standard Headers.
'
Imports System.Net
Imports Lextm.SharpSnmpLib.Messaging

Module Program
    Public Sub Main(ByVal args As String())
        If args.Length <> 0 Then
            Return
        End If

        Console.WriteLine("#SNMP is available at http://sharpsnmplib.codeplex.com")
        Dim watcher As New Listener()
        Dim adapter As New DefaultListenerAdapter()
        watcher.Adapters.Add(adapter)
        AddHandler adapter.TrapV1Received, AddressOf WatcherTrapV1Received
        AddHandler adapter.TrapV2Received, AddressOf WatcherTrapV2Received
        AddHandler adapter.InformRequestReceived, AddressOf WatcherInformRequestReceived
        watcher.AddBinding(New IPEndPoint(IPAddress.Any, 162))
        watcher.Start()
        Console.WriteLine("Press any key to stop . . . ")
        Console.Read()
    End Sub

    Private Sub WatcherInformRequestReceived(ByVal sender As Object, ByVal e As MessageReceivedEventArgs(Of InformRequestMessage))
        Console.WriteLine(e)
    End Sub

    Private Sub WatcherTrapV2Received(ByVal sender As Object, ByVal e As MessageReceivedEventArgs(Of TrapV2Message))
        Console.WriteLine(e)
    End Sub

    Private Sub WatcherTrapV1Received(ByVal sender As Object, ByVal e As MessageReceivedEventArgs(Of TrapV1Message))
        Console.WriteLine(e)
    End Sub
End Module
