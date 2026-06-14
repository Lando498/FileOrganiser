Imports System.IO
Imports System.Runtime.InteropServices

Public Class Form1
    Dim folder = Nothing
    Dim contents1 As List(Of String)
    Dim contents2 As List(Of String)
    Dim organised As Integer = 0
    Dim x = 0
    Dim knownFiles As New HashSet(Of String)

    Private Sub organise(newf As String)

        If Not File.Exists(newf) Then Exit Sub

        Try
            Dim extension As String = Path.GetExtension(newf).TrimStart("."c)
            Dim targetFolder As String = Path.Combine(folder, extension)

            If Not Directory.Exists(targetFolder) Then
                Directory.CreateDirectory(targetFolder)
            End If

            Dim destination As String = Path.Combine(targetFolder, Path.GetFileName(newf))

            File.Move(newf, destination)

            organised += 1
            File.WriteAllText("organised.txt", organised.ToString())

            Debug.WriteLine("Moved: " & newf)

        Catch ex As Exception
            Debug.WriteLine("ERROR: " & ex.Message)
        End Try

    End Sub


    Private Sub Form1_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Debug.WriteLine("Form loaded")
        For Each Dir As String In Directory.GetDirectories("c:\")
            Debug.WriteLine(Dir)
        Next
        If File.Exists("folder.txt") Then
            folder = File.ReadAllText("folder.txt")
        Else
            File.Create("folder.txt").Dispose()
        End If
        If File.Exists("organised.txt") Then
            organised = Integer.Parse(File.ReadAllText("organised.txt"))
        Else
            File.Create("organised.txt").Dispose()
        End If
    End Sub

    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        If FolderBrowserDialog1.ShowDialog() = DialogResult.OK Then
            folder = FolderBrowserDialog1.SelectedPath
            File.WriteAllText("folder.txt", FolderBrowserDialog1.SelectedPath)
        End If
    End Sub

    Private Sub Timer1_Tick(sender As Object, e As EventArgs) Handles Timer1.Tick
        Label4.Text = organised.ToString()
        If String.IsNullOrWhiteSpace(folder) Then Exit Sub

        Dim files = Directory.GetFiles(folder)

        For Each f In files
            If Not knownFiles.Contains(f) Then
                knownFiles.Add(f)

                Debug.WriteLine("New file detected: " & f)
                organise(f)
            End If
        Next

    End Sub
End Class