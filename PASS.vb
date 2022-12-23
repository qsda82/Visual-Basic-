Public Class PASS
    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        If Button1.Tag = 1 Then
            L1P2.Show()
            Me.Close()
        End If
        If Button1.Tag = 2 Then
            L1P3.Show()
            Me.Close()
        End If

        If Button1.Tag = 3 Then
            MsgBox("難度提升至 高手 ")
            L2P1.Show()
            Me.Close()
        End If

        If Button1.Tag = 4 Then
            L2P2.Show()
            Me.Close()
        End If

        If Button1.Tag = 5 Then
            L2P3.Show()
            Me.Close()
        End If

        If Button1.Tag = 6 Then
            L3P1.Show()
            Me.Close()
        End If


    End Sub

    Private Sub Button2_Click(sender As Object, e As EventArgs) Handles Button2.Click
        If Button1.Tag = 1 Then
            L1P1.Show()
            Me.Close()
        End If

        If Button1.Tag = 2 Then
            L1P2.Show()
            Me.Close()
        End If

        If Button1.Tag = 3 Then
            L1P3.Show()
            Me.Close()
        End If

        If Button1.Tag = 4 Then
            L2P1.Show()
            Me.Close()
        End If

        If Button1.Tag = 5 Then
            L2P2.Show()
            Me.Close()
        End If

        If Button1.Tag = 6 Then
            L2P3.Show()
            Me.Close()
        End If


    End Sub

    Private Sub Label1_Click(sender As Object, e As EventArgs) Handles Label1.Click

    End Sub

    Private Sub PictureBox1_Click(sender As Object, e As EventArgs) Handles PictureBox1.Click

    End Sub
End Class