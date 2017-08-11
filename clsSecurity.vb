Imports Microsoft.VisualBasic
Imports Microsoft.VisualBasic.CompilerServices
Imports System
Imports System.IO
Imports System.Security.Cryptography
Imports System.Text

Public Class clsSecurity
    Public Sub New()

    End Sub
    ' Methods
    Private Shared Function a(ByVal A_0 As String) As String
        Dim str As String
        Try
            Dim bytes As Byte() = Encoding.UTF8.GetBytes(A_0)
            Dim provider As New MD5CryptoServiceProvider
            str = Strings.Replace(BitConverter.ToString(provider.ComputeHash(bytes)), "-", "", 1, -1, CompareMethod.Binary)
        Catch exception1 As Exception
            ProjectData.SetProjectError(exception1)
            Dim exception As Exception = exception1
            str = Nothing
            ProjectData.ClearProjectError()
        End Try
        Return str
    End Function

    Public Shared Function Decrypt(ByVal EncryptedString As String, ByVal Key As String) As String
        Dim message As String
        Try
            Dim buffer4 As Byte() = Convert.FromBase64String(EncryptedString)
            Dim bytes As Byte() = Encoding.UTF8.GetBytes(clsSecurity.a(Key))
            Dim rgbIV As Byte() = New Byte() {&H52, &H25, 9, &HEB, &H89, &H38, &H83, 220, 15, &HDF, &HAC, &H95, &H7C, &H86, &HF8, 7}
            Dim buffer As Byte() = New Byte(((buffer4.Length - 1) + 1) - 1) {}
            Dim managed As New RijndaelManaged
            Dim stream2 As New MemoryStream(buffer4)
            Dim transform As ICryptoTransform = managed.CreateDecryptor(bytes, rgbIV)
            Dim stream As New CryptoStream(stream2, transform, CryptoStreamMode.Read)
            stream.Read(buffer, 0, buffer.Length)
            stream2.Close()
            stream.Close()
            message = Encoding.UTF8.GetString(buffer).Replace(ChrW(0), "")
        Catch exception1 As Exception
            ProjectData.SetProjectError(exception1)
            Dim exception As Exception = exception1
            message = exception.Message
            ProjectData.ClearProjectError()
        End Try
        Return message
    End Function

    Public Shared Function Encrypt(ByVal StrText As String, ByVal Key As String) As String
        Dim str As String
        Try
            Dim str3 As String = StrText.Replace(ChrW(0), "")
            Dim bytes As Byte() = Encoding.UTF8.GetBytes(StrText)
            Dim rgbKey As Byte() = Encoding.UTF8.GetBytes(clsSecurity.a(Key))
            Dim rgbIV As Byte() = New Byte() {&H52, &H25, 9, &HEB, &H89, &H38, &H83, 220, 15, &HDF, &HAC, &H95, &H7C, &H86, &HF8, 7}
            Dim managed As New RijndaelManaged
            Dim stream2 As New MemoryStream
            Dim transform As ICryptoTransform = managed.CreateEncryptor(rgbKey, rgbIV)
            Dim stream As New CryptoStream(stream2, transform, CryptoStreamMode.Write)
            stream.Write(bytes, 0, bytes.Length)
            stream.FlushFinalBlock()
            Dim str2 As String = Convert.ToBase64String(stream2.ToArray)
            stream2.Close()
            stream.Close()
            str = str2
        Catch exception1 As Exception
            ProjectData.SetProjectError(exception1)
            Dim exception As Exception = exception1
            str = Nothing
            ProjectData.ClearProjectError()
        End Try
        Return str
    End Function

End Class


