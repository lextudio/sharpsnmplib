﻿'
' Created by SharpDevelop.
' User: Administrator
' Date: 2010/4/25
' Time: 14:13
'
' To change this template use Tools | Options | Coding | Edit Standard Headers.
'
Imports Lextm.SharpSnmpLib
Imports Lextm.SharpSnmpLib.Security
Imports Lextm.SharpSnmpLib.Messaging
Imports Mono.Options
Imports System.Net
Imports System.Net.Sockets

Module Program
    Public Sub Main(ByVal args As String())
        Dim community As String = "public"
        Dim showHelp__1 As Boolean = False
        Dim showVersion As Boolean = False
        Dim version As VersionCode = VersionCode.V1
        Dim timeout As Integer = 1000
        Dim retry As Integer = 0
        Dim level As Levels = Levels.Reportable
        Dim user As String = String.Empty
        Dim contextName As String = String.Empty
        Dim authentication As String = String.Empty
        Dim authPhrase As String = String.Empty
        Dim privacy As String = String.Empty
        Dim privPhrase As String = String.Empty
        Dim dump As Boolean = False

        Dim p As OptionSet = New OptionSet().Add("c:", "Community name, (default is public)", Sub(v As String)
                                                                                                  If v IsNot Nothing Then
                                                                                                      community = v
                                                                                                  End If
                                                                                              End Sub) _
                                            .Add("l:", "Security level, (default is noAuthNoPriv)", Sub(v As String)
                                                                                                        If v.ToUpperInvariant() = "NOAUTHNOPRIV" Then
                                                                                                            level = Levels.Reportable
                                                                                                        ElseIf v.ToUpperInvariant() = "AUTHNOPRIV" Then
                                                                                                            level = Levels.Authentication Or Levels.Reportable
                                                                                                        ElseIf v.ToUpperInvariant() = "AUTHPRIV" Then
                                                                                                            level = Levels.Authentication Or Levels.Privacy Or Levels.Reportable
                                                                                                        Else
                                                                                                            Throw New ArgumentException("no such security mode: " & v)
                                                                                                        End If
                                                                                                    End Sub) _
                                            .Add("a:", "Authentication method (MD5 or SHA)", Sub(v As String)
                                                                                                 authentication = v
                                                                                             End Sub) _
                                            .Add("A:", "Authentication passphrase", Sub(v As String)
                                                                                        authPhrase = v
                                                                                    End Sub) _
                                            .Add("x:", "Privacy method", Sub(v As String)
                                                                             privacy = v
                                                                         End Sub) _
                                            .Add("X:", "Privacy passphrase", Sub(v As String)
                                                                                 privPhrase = v
                                                                             End Sub) _
                                            .Add("u:", "Security name", Sub(v As String)
                                                                            user = v
                                                                        End Sub) _
                                            .Add("C:", "Context name", Sub(v As String)
                                                                            contextName = v
                                                                       End Sub) _
                                            .Add("h|?|help", "Print this help information.", Sub(v As String)
                                                                                                 showHelp__1 = v IsNot Nothing
                                                                                             End Sub) _
                                            .Add("V", "Display version number of this application.", Sub(v As String)
                                                                                                         showVersion = v IsNot Nothing
                                                                                                     End Sub) _
                                            .Add("d", "Display message dump", Sub(v As String)
                                                                                  dump = True
                                                                              End Sub) _
                                            .Add("t:", "Timeout value (unit is second).", Sub(v As String)
                                                                                              timeout = Integer.Parse(v) * 1000
                                                                                          End Sub) _
                                            .Add("r:", "Retry count (default is 0)", Sub(v As String)
                                                                                         retry = Integer.Parse(v)
                                                                                     End Sub) _
                                            .Add("v:", "SNMP version (1, 2, and 3 are currently supported)", Sub(v As String)
                                                                                                                 Select Case Integer.Parse(v)
                                                                                                                     Case 1
                                                                                                                         version = VersionCode.V1
                                                                                                                         Exit Select
                                                                                                                     Case 2
                                                                                                                         version = VersionCode.V2
                                                                                                                         Exit Select
                                                                                                                     Case 3
                                                                                                                         version = VersionCode.V3
                                                                                                                         Exit Select
                                                                                                                     Case Else
                                                                                                                         Throw New ArgumentException("no such version: " & v)
                                                                                                                 End Select
                                                                                                             End Sub)

        If args.Length = 0 Then
            ShowHelp(p)
            Return
        End If

        Dim extra As List(Of String)
        Try
            extra = p.Parse(args)
        Catch ex As OptionException
            Console.WriteLine(ex.Message)
            Return
        End Try

        If showHelp__1 Then
            ShowHelp(p)
            Return
        End If

        If extra.Count < 2 Then
            Console.WriteLine("invalid variable number: " & extra.Count)
            Return
        End If

        If showVersion Then
            Console.WriteLine(Reflection.Assembly.GetExecutingAssembly().GetName().Version)
            Return
        End If

        Dim ip As IPAddress
        Dim parsed As Boolean = IPAddress.TryParse(extra(0), ip)
        If Not parsed Then
            For Each address As IPAddress In Dns.GetHostAddresses(extra(0))
                If address.AddressFamily <> AddressFamily.InterNetwork Then
                    Continue For
                End If

                ip = address
                Exit For
            Next

            If ip Is Nothing Then
                Console.WriteLine("invalid host or wrong IP address found: " & extra(0))
                Return
            End If
        End If

        Try
            Dim vList As New List(Of Variable)()
            For i As Integer = 1 To extra.Count - 1
                Dim test As New Variable(New ObjectIdentifier(extra(i)))
                vList.Add(test)
            Next

            Dim receiver As New IPEndPoint(ip, 161)
            If version <> VersionCode.V3 Then
                Dim message As New GetNextRequestMessage(0, version, New OctetString(community), vList)
                Dim response As ISnmpMessage = message.GetResponse(timeout, receiver)
                If response.Pdu.ErrorStatus.ToInt32() <> 0 Then
                    ' != ErrorCode.NoError
                    Throw ErrorException.Create("error in response", receiver.Address, response)
                End If

                For Each variable As Variable In response.Pdu.Variables
                    Console.WriteLine(variable)
                Next

                Return
            End If

            If String.IsNullOrEmpty(user) Then
                Console.WriteLine("User name need to be specified for v3.")
                Return
            End If

            Dim auth As IAuthenticationProvider = If((level And Levels.Authentication) = Levels.Authentication, GetAuthenticationProviderByName(authentication, authPhrase), DefaultAuthenticationProvider.Instance)

            Dim priv As IPrivacyProvider
            If ((level And Levels.Privacy) = Levels.Privacy) Then
                priv = New DESPrivacyProvider(New OctetString(privPhrase), auth)
            Else
                priv = New DefaultPrivacyProvider(auth)
            End If

            Dim report As ReportMessage = Messenger.GetNextDiscovery(SnmpType.GetNextRequestPdu).GetResponse(timeout, receiver)

            Dim request As New GetNextRequestMessage(VersionCode.V3, Messenger.NextMessageId, Messenger.NextRequestId, New OctetString(user), New OctetString(IF(string.IsNullOrWhiteSpace(contextName), String.Empty, contextName)), vList, priv, Messenger.MaxMessageSize, _
             report)

            Dim reply As ISnmpMessage = request.GetResponse(timeout, receiver)
            If dump Then
                Console.WriteLine("Request message bytes:")
                Console.WriteLine(ByteTool.Convert(request.ToBytes()))
                Console.WriteLine("Response message bytes:")
                Console.WriteLine(ByteTool.Convert(reply.ToBytes()))
            End If

            If TypeOf reply Is ReportMessage Then
                If reply.Pdu().Variables.Count = 0 Then
                    Console.WriteLine("wrong report message received")
                    Return
                End If

                Dim id As ObjectIdentifier = reply.Pdu().Variables(0).Id
                If id <> Messenger.NotInTimeWindow Then
                    Dim errorMessage As String = id.GetErrorMessage()
                    Console.WriteLine(errorMessage)
                    Return
                End If

                ' according to RFC 3414, send a second request to sync time.
                request = New GetNextRequestMessage(VersionCode.V3, Messenger.NextMessageId, Messenger.NextRequestId, New OctetString(user), New OctetString(IF(string.IsNullOrWhiteSpace(contextName), String.Empty, contextName)), vList, priv, Messenger.MaxMessageSize, _
             reply)
                reply = request.GetResponse(timeout, receiver)
            ElseIf reply.Pdu.ErrorStatus.ToInt32() <> 0 Then
                ' != ErrorCode.NoError
                Throw ErrorException.Create("error in response", receiver.Address, reply)
            End If

            For Each v As Variable In reply.Pdu.Variables
                Console.WriteLine(v)
            Next
        Catch ex As SnmpException
            Console.WriteLine(ex)
        Catch ex As SocketException
            Console.WriteLine(ex)
        End Try
    End Sub

    Private Function GetAuthenticationProviderByName(ByVal authentication As String, ByVal phrase As String) As IAuthenticationProvider
        If authentication.ToUpperInvariant() = "MD5" Then
            Return New MD5AuthenticationProvider(New OctetString(phrase))
        End If

        If authentication.ToUpperInvariant() = "SHA" Then
            Return New SHA1AuthenticationProvider(New OctetString(phrase))
        End If

        Throw New ArgumentException("unknown name", NameOf(authentication))
    End Function

    Private Sub ShowHelp(ByRef optionSet As OptionSet)
        Console.WriteLine("#SNMP is available at https://sharpsnmp.com")
        Console.WriteLine("snmpgetnext [Options] IP-address|host-name OID [OID] ...")
        Console.WriteLine("Options:")
        optionSet.WriteOptionDescriptions(Console.Out)
    End Sub
End Module
