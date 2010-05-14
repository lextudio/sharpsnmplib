Imports Lextm.SharpSnmpLib.Mib
Imports Lextm.SharpSnmpLib

Module Program

    Sub Main(ByVal args As String())
        If args.Length <> 1 Then
            Console.WriteLine("This application takes one parameter.")
        End If

        Dim oid As String = args(0)
        Dim registry As New ReloadableObjectRegistry("modules")
        Dim textual As String = registry.Translate(ObjectIdentifier.Convert(oid))
        Console.WriteLine(textual)
    End Sub

End Module
