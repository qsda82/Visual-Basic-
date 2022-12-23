Public Class L2P3
    Dim check As Integer = 0
    Dim spacecheck As Boolean = True
    Dim xx As Boolean = True
    Private Sub Form2_KeyDown(sender As Object, e As System.Windows.Forms.KeyEventArgs) Handles Me.KeyDown
        If (e.KeyValue = Keys.Space) Then
            Timer1.Start() '啟動球的運動
            Timer2.Enabled = True
            spacecheck = False
        End If
        If ptimes = 1 Then
            Timer1.Enabled = False

        End If
    End Sub

    Dim Dietimes As Integer = 0

    Private Sub Timer1_Tick(sender As Object, e As EventArgs) Handles Timer1.Tick
        For i As Integer = 1 To ball_num
            If_dead()
            Ball_move(ball(i))
            If_Win(ball(i))
        Next

    End Sub

    Function Ball_move(B As Label) As Boolean
        Dim tmp As Integer = B.Tag
        If tmp = 0 Then
            Return False
        End If

        B.Left = B.Left + vx(tmp) 'X方向移動
        B.Top = B.Top + vy(tmp)  'Y方向移動

        If B.Left < 0 Then vx(tmp) = Math.Abs(vx(tmp)) '碰左牆

        If B.Right > Me.ClientSize.Width Then vx(tmp) = -Math.Abs(vx(tmp)) '碰右牆

        If B.Top < 60 Then '碰頂端
            vy(tmp) = Math.Abs(vy(tmp))
        End If

        Dim BCenter As Single = (B.Left + B.Right) / 2 '球中心點X座標

        If B.Bottom > P.Top And BCenter > P.Left And BCenter < P.Right Then
            vy(tmp) = -Math.Abs(vy(tmp)) '像上彈
            Dim BHitPoint As Single = (BCenter - P.Left) / P.Width '計算擊球點
            If BHitPoint > 0.66 Then
                vx(tmp) = 5
                vy(tmp) = -5
            End If

            If BHitPoint < 0.33 Then
                vx(tmp) = -5
                vy(tmp) = -5
            End If
        ElseIf (B.Bottom > P.Top And BCenter > P.Left) Or (B.Bottom > P.Top And BCenter < P.Right) Then
            vy(tmp) = Math.Abs(vy(tmp))
        ElseIf (B.Bottom > P.Bottom And BCenter > P.Left) Or (B.Bottom > P.Bottom And BCenter < P.Right) Then
            vy(tmp) = Math.Abs(vy(tmp))

        End If
        Return True
    End Function

    Function If_dead() As Boolean
        Dim dead As Boolean = True
        For i As Integer = 1 To ball_num
            If ball(i).Location.Y < Me.ClientSize.Height Then
                dead = False
            Else
                ball(i).Enabled = False
                ball(i).Visible = False
                ball(i).Tag = 0
            End If
        Next
        If dead = False Then Return False
        Dietimes = Dietimes + 1
        Timer1.Stop()
        Timer2.Enabled = False
        Timer3.Enabled = False
        Timer4.Enabled = False
        For Each Ctl As Control In Me.Controls
            If TypeOf Ctl Is PictureBox Then
                Ctl.Visible = True
            End If
        Next
        For Each Ctl As Control In Me.Controls
            If TypeOf Ctl Is Label Then
                Ctl.Visible = True
            End If
        Next
        Dim q As Integer = 1
        Do While q <> 10
            For Each Ctl As Control In Me.Controls
                If TypeOf Ctl Is PictureBox Then
                    For x As Integer = 1 To 4
                        If Ctl.Tag = x Then
                            Ctl.Dispose()
                        End If
                    Next
                End If
            Next
            q = q + 1
        Loop


        If Dietimes = 1 Then
            Label6.Image = Nothing
            MsgBox("你死了，還剩2次機會")
            P.Top = 625 '調整球拍的y座標
            P.Left = (Me.ClientSize.Width - P.Width) / 2 '球拍水平置中
            spacecheck = True
            New_ball()
        ElseIf Dietimes = 2 Then
            Label6.Image = Nothing
            Label5.Image = Nothing
            MsgBox("你死了，還剩1次機會")
            P.Top = 625 '調整球拍的y座標
            P.Left = (Me.ClientSize.Width - P.Width) / 2 '球拍水平置中
            spacecheck = True
            New_ball()
        ElseIf Dietimes = 3 Then
            Label6.Image = Nothing
            Label5.Image = Nothing
            Label4.Image = Nothing
            YOUDIE.Button1.Tag = 6
            YOUDIE.Show()
            Me.Close()
        End If

        Return True
    End Function

    Function If_Win(B As Label) As Boolean
        If xx = False Then
            check = 0
            xx = True
        End If
        For Each q In Me.Controls
            If TypeOf (q) Is PictureBox AndAlso chkHit(q, B) Then
                check = check + 1
            End If
            If check = 263 Then

                Timer1.Stop()
                PASS.Button1.Tag = 6
                PASS.Show()
                Me.Close()

            End If
        Next


        Return True
    End Function


    Function chkHit(Q As PictureBox, Bal As Label) As Boolean
        Dim tmp As Integer = Bal.Tag
        If tmp = 0 Then
            Return False
        End If
        If Bal.Right < Q.Left Then Return False '偏左為碰到
        If Bal.Left > Q.Right Then Return False '偏右為碰到 
        If Bal.Top > Q.Bottom Then Return False '偏下為碰到
        If Bal.Bottom < Q.Top Then Return False '偏上為碰到 '碰撞目標左側(剛剛越過左邊界)左轉彎                           
        If Bal.Right >= Q.Left And (Bal.Right - Q.Left) <= Math.Abs(vx(tmp)) Then vx(tmp) = -Math.Abs(vx(tmp)) '碰撞目標右側(剛剛越過左邊界)右轉彎                           
        If Bal.Left <= Q.Right And (Q.Right - Bal.Left) <= Math.Abs(vx(tmp)) Then vx(tmp) = Math.Abs(vx(tmp)) '碰撞目標底部(剛剛越過左邊界)往下彈
        If Bal.Top <= Q.Bottom And (Q.Bottom - Bal.Top) <= Math.Abs(vy(tmp)) Then vy(tmp) = Math.Abs(vy(tmp)) '碰撞目標頂部(剛剛越過左邊界)往上彈
        If Bal.Bottom >= Q.Top And (Bal.Bottom - Q.Top) <= Math.Abs(vy(tmp)) Then vy(tmp) = -Math.Abs(vy(tmp))
        If Q.Tag = 1 Then

            Timer3.Enabled = True
            Timer4.Enabled = False
            check = check - 1

        ElseIf Q.Tag = 2 Then
            P.Width = P.Width + 20
            check = check - 1

        ElseIf Q.Tag = 3 Then

            Timer4.Enabled = True
            Timer3.Enabled = False
            check = check - 1

        ElseIf Q.Tag = 4 Then
            check = check - 1
            New_ball()
        End If
        Q.Dispose() '刪除磚塊
        Return True '回傳有碰撞
    End Function

    Dim ball() As Label
    Dim ball_num As Integer = 0
    Dim vx() As Single
    Dim vy() As Single
    Function New_ball() As Integer
        Dim x As Integer = P.Location.X
        ball_num += 1
        ReDim Preserve ball(ball_num)
        ReDim Preserve vx(ball_num)
        vx(ball_num) = 0
        ReDim Preserve vy(ball_num)
        vy(ball_num) = 5
        ball(ball_num) = New Label()
        With ball(ball_num)
            .Location = New Point(P.Location.X + P.Size.Width / 2 - 15, 595)
            .Size = New Size(28, 28)
            .Image = My.Resources._6663
            .Visible = True
            .Tag = ball_num
        End With

        Me.Controls.Add(ball(ball_num))
        ball(ball_num).BringToFront()
        Return ball_num
    End Function

    Dim mdx As Integer

    Private Sub p_mousedown(sender As Object, e As MouseEventArgs) Handles P.MouseDown
        mdx = e.X '紀錄拖移起點
    End Sub
    Dim dis As Integer

    Function bmove(bal As Label)
        bal.Left = dis + 50
    End Function

    Private Sub p_mousemove(sender As Object, e As MouseEventArgs) Handles P.MouseMove
        If e.Button = Windows.Forms.MouseButtons.Left Then
            Dim X As Integer = P.Left + (e.X - mdx) '試算拖曳位置
            If X < 0 Then X = 0 '左極限控制
            If X > Me.ClientSize.Width - P.Width Then
                X = Me.ClientSize.Width - P.Width '右極限控制
            End If
            P.Left = X '球拍位置(不超出邊界)
            dis = X
            If spacecheck = True Then
                For i As Integer = 1 To ball_num
                    bmove(ball(i))
                Next
            End If
        End If
    End Sub
    Dim ptimes As Integer = 0
    Dim le As Boolean
    Dim c As Integer = 0
    Dim b As Integer = 0
    Private Sub KeyDown1(sender As Object, e As System.Windows.Forms.KeyEventArgs) Handles Me.KeyDown

        '暫停功能
        If spacecheck = False Then
            If (e.KeyValue = Keys.P) Then
                If ptimes = 0 Then
                    Timer1.Stop()
                    Timer2.Enabled = False

                    If Timer3.Enabled = True Then
                        c = 1
                        Timer3.Enabled = False
                    End If

                    If Timer4.Enabled = True Then
                        b = 1
                        Timer4.Enabled = False
                    End If

                    ptimes = ptimes + 1
                    P.Enabled = False
                ElseIf ptimes = 1 Then
                    Timer1.Start()
                    Timer2.Enabled = True
                    If c = 1 Then
                        Timer3.Enabled = True
                        c = 0
                    End If
                    If b = 1 Then
                        Timer4.Enabled = True
                        b = 0
                    End If
                    ptimes = ptimes - 1
                    P.Enabled = True
                End If
            End If
        End If

    End Sub

    Private Sub Label2_Click(sender As Object, e As EventArgs) Handles Label2.Click
        EXPLAN.Show()
    End Sub

    Function DELETE(bal As Label)
        bal.Top = 1000
    End Function

    Private Sub Label3_Click(sender As Object, e As EventArgs) Handles Label3.Click
        Timer1.Enabled = False
        Timer2.Enabled = False
        Timer3.Enabled = False
        Timer4.Enabled = False
        P.Enabled = True
        ptimes = 0
        Label6.Image = My.Resources.love
        Label5.Image = My.Resources.love
        Label4.Image = My.Resources.love
        Dietimes = 0
        xx = False
        n = 0
        t = 0
        Dim ee As Integer = 1
        Do While ee <> 10
            For Each Ctl1 As Control In Me.Controls
                If TypeOf Ctl1 Is PictureBox Then
                    Ctl1.Dispose()
                End If
            Next
            ee = ee + 1
        Loop
        Dim q As Integer = 1
        Do While q <> 10
            For Each Ctl As Control In Me.Controls
                If TypeOf Ctl Is PictureBox Then
                    For x As Integer = 1 To 4
                        If Ctl.Tag = x Then
                            Ctl.Dispose()
                        End If
                    Next
                End If
            Next
            q = q + 1
        Loop
        For a As Integer = 1 To ball_num
            DELETE(ball(a))
        Next
        P.Top = 625 '調整球拍的y座標
        P.Left = (Me.ClientSize.Width - P.Width) / 2 '球拍水平置中
        P.Width = 131
        spacecheck = True
        New_ball()
        Dim i As Integer
        Dim pic1(9) As PictureBox
        For i = 0 To 8
            If pic1(i) Is Nothing Then
                pic1(i) = New PictureBox
                Me.Controls.Add(pic1(i))
            End If

            With pic1(i)
                .Enabled = True
                .Location = New Point(48, 203 + (i * 21))
                .Size = New Size(51, 20)
                .BackColor = Color.Black
                .BorderStyle = BorderStyle.FixedSingle
            End With
        Next

        Dim pic2(20) As PictureBox
        For i = 0 To 19
            If pic2(i) Is Nothing Then
                pic2(i) = New PictureBox
                Me.Controls.Add(pic2(i))
            End If

            With pic2(i)
                .Enabled = True
                .Location = New Point(100, 56 + (i * 21))
                .Size = New Size(51, 20)
                .BackColor = Color.Black
                .BorderStyle = BorderStyle.FixedSingle
            End With
        Next
        pic2(0).Dispose()
        pic2(18).Dispose()
        pic2(19).Dispose()
        pic2(11).BackColor = Color.BlanchedAlmond
        pic2(12).BackColor = Color.BlanchedAlmond
        pic2(13).BackColor = Color.BlanchedAlmond
        pic2(14).BackColor = Color.BlanchedAlmond
        pic2(15).BackColor = Color.BlanchedAlmond
        pic2(7).BackColor = Color.CadetBlue
        pic2(8).BackColor = Color.CadetBlue
        pic2(9).BackColor = Color.CadetBlue
        pic2(10).BackColor = Color.CadetBlue

        Dim pic3(20) As PictureBox
        For i = 0 To 19
            If pic3(i) Is Nothing Then
                pic3(i) = New PictureBox
                Me.Controls.Add(pic3(i))
            End If

            With pic3(i)
                .Enabled = True
                .Location = New Point(152, 56 + (i * 21))
                .Size = New Size(51, 20)
                .BackColor = Color.CadetBlue
                .BorderStyle = BorderStyle.FixedSingle
            End With
        Next
        pic3(19).Dispose()
        pic3(0).BackColor = Color.Black
        pic3(18).BackColor = Color.Black
        pic3(10).BackColor = Color.BlanchedAlmond
        pic3(11).BackColor = Color.BlanchedAlmond
        pic3(12).BackColor = Color.BlanchedAlmond
        pic3(13).BackColor = Color.BlanchedAlmond
        pic3(14).BackColor = Color.BlanchedAlmond
        pic3(15).BackColor = Color.BlanchedAlmond
        pic3(16).BackColor = Color.Peru
        pic3(17).BackColor = Color.Peru

        Dim pic4(20) As PictureBox
        For i = 0 To 19
            If pic4(i) Is Nothing Then
                pic4(i) = New PictureBox
                Me.Controls.Add(pic4(i))
            End If

            With pic4(i)
                .Enabled = True
                .Location = New Point(204, 56 + (i * 21))
                .Size = New Size(51, 20)
                .BackColor = Color.CadetBlue
                .BorderStyle = BorderStyle.FixedSingle
            End With
        Next
        pic4(0).BackColor = Color.Black
        pic4(19).BackColor = Color.Black
        pic4(14).BackColor = Color.Black
        pic4(18).BackColor = Color.Black
        pic4(9).BackColor = Color.BlanchedAlmond
        pic4(10).BackColor = Color.BlanchedAlmond
        pic4(11).BackColor = Color.BlanchedAlmond
        pic4(12).BackColor = Color.BlanchedAlmond
        pic4(13).BackColor = Color.BlanchedAlmond
        pic4(16).BackColor = Color.Peru
        pic4(17).BackColor = Color.Peru

        Dim pic5(20) As PictureBox
        For i = 0 To 19
            If pic5(i) Is Nothing Then
                pic5(i) = New PictureBox
                Me.Controls.Add(pic5(i))
            End If

            With pic5(i)
                .Enabled = True
                .Location = New Point(256, 56 + (i * 21))
                .Size = New Size(51, 20)
                .BackColor = Color.CadetBlue
                .BorderStyle = BorderStyle.FixedSingle
            End With
        Next
        pic5(0).Dispose()
        pic5(1).BackColor = Color.Black
        pic5(19).BackColor = Color.Black
        pic5(15).BackColor = Color.Black
        pic5(14).BackColor = Color.Black
        pic5(8).BackColor = Color.BlanchedAlmond
        pic5(9).BackColor = Color.BlanchedAlmond
        pic5(10).BackColor = Color.BlanchedAlmond
        pic5(11).BackColor = Color.BlanchedAlmond
        pic5(12).BackColor = Color.BlanchedAlmond
        pic5(13).BackColor = Color.BlanchedAlmond
        pic5(16).BackColor = Color.BlanchedAlmond
        pic5(17).BackColor = Color.BlanchedAlmond
        pic5(18).BackColor = Color.BlanchedAlmond

        Dim pic6(20) As PictureBox
        For i = 0 To 19
            If pic6(i) Is Nothing Then
                pic6(i) = New PictureBox
                Me.Controls.Add(pic6(i))
            End If

            With pic6(i)
                .Enabled = True
                .Location = New Point(308, 56 + (i * 21))
                .Size = New Size(51, 20)
                .BackColor = Color.CadetBlue
                .BorderStyle = BorderStyle.FixedSingle
            End With
        Next
        pic6(0).Dispose()
        pic6(1).BackColor = Color.Black
        pic6(19).BackColor = Color.Black
        pic6(15).BackColor = Color.Black
        pic6(14).BackColor = Color.Black
        pic6(2).BackColor = Color.Black
        pic6(9).BackColor = Color.BlanchedAlmond
        pic6(10).BackColor = Color.BlanchedAlmond
        pic6(11).BackColor = Color.BlanchedAlmond
        pic6(12).BackColor = Color.BlanchedAlmond
        pic6(13).BackColor = Color.BlanchedAlmond
        pic6(16).BackColor = Color.BlanchedAlmond
        pic6(17).BackColor = Color.BlanchedAlmond
        pic6(18).BackColor = Color.BlanchedAlmond

        Dim pic7(20) As PictureBox
        For i = 0 To 19
            If pic7(i) Is Nothing Then
                pic7(i) = New PictureBox
                Me.Controls.Add(pic7(i))
            End If

            With pic7(i)
                .Enabled = True
                .Location = New Point(360, 56 + (i * 21))
                .Size = New Size(51, 20)
                .BackColor = Color.CadetBlue
                .BorderStyle = BorderStyle.FixedSingle
            End With
        Next
        pic7(0).Dispose()
        pic7(1).BackColor = Color.Black
        pic7(19).BackColor = Color.Black
        pic7(10).BackColor = Color.BlanchedAlmond
        pic7(11).BackColor = Color.BlanchedAlmond
        pic7(12).BackColor = Color.BlanchedAlmond
        pic7(13).BackColor = Color.BlanchedAlmond
        pic7(14).BackColor = Color.BlanchedAlmond
        pic7(15).BackColor = Color.BlanchedAlmond
        pic7(16).BackColor = Color.BlanchedAlmond
        pic7(17).BackColor = Color.BlanchedAlmond
        pic7(18).BackColor = Color.BlanchedAlmond

        Dim pic8(20) As PictureBox
        For i = 0 To 19
            If pic8(i) Is Nothing Then
                pic8(i) = New PictureBox
                Me.Controls.Add(pic8(i))
            End If

            With pic8(i)
                .Enabled = True
                .Location = New Point(412, 56 + (i * 21))
                .Size = New Size(51, 20)
                .BackColor = Color.CadetBlue
                .BorderStyle = BorderStyle.FixedSingle
            End With
        Next
        pic8(0).Dispose()
        pic8(1).BackColor = Color.Black
        pic8(19).BackColor = Color.Black
        pic8(11).BackColor = Color.BlanchedAlmond
        pic8(12).BackColor = Color.BlanchedAlmond
        pic8(13).BackColor = Color.BlanchedAlmond
        pic8(14).BackColor = Color.BlanchedAlmond
        pic8(15).BackColor = Color.BlanchedAlmond
        pic8(16).BackColor = Color.BlanchedAlmond
        pic8(17).BackColor = Color.BlanchedAlmond
        pic8(18).BackColor = Color.BlanchedAlmond

        Dim pic9(20) As PictureBox
        For i = 0 To 19
            If pic9(i) Is Nothing Then
                pic9(i) = New PictureBox
                Me.Controls.Add(pic9(i))
            End If

            With pic9(i)
                .Enabled = True
                .Location = New Point(464, 56 + (i * 21))
                .Size = New Size(51, 20)
                .BackColor = Color.CadetBlue
                .BorderStyle = BorderStyle.FixedSingle
            End With
        Next
        pic9(0).Dispose()
        pic9(1).BackColor = Color.Black
        pic9(19).BackColor = Color.Black
        pic9(10).BackColor = Color.BlanchedAlmond
        pic9(11).BackColor = Color.BlanchedAlmond
        pic9(12).BackColor = Color.BlanchedAlmond
        pic9(13).BackColor = Color.BlanchedAlmond
        pic9(14).BackColor = Color.BlanchedAlmond
        pic9(15).BackColor = Color.BlanchedAlmond
        pic9(16).BackColor = Color.BlanchedAlmond
        pic9(17).BackColor = Color.BlanchedAlmond
        pic9(18).BackColor = Color.BlanchedAlmond

        Dim pic10(20) As PictureBox
        For i = 0 To 19
            If pic10(i) Is Nothing Then
                pic10(i) = New PictureBox
                Me.Controls.Add(pic10(i))
            End If

            With pic10(i)
                .Enabled = True
                .Location = New Point(516, 56 + (i * 21))
                .Size = New Size(51, 20)
                .BackColor = Color.CadetBlue
                .BorderStyle = BorderStyle.FixedSingle
            End With
        Next
        pic10(0).Dispose()
        pic10(1).BackColor = Color.Black
        pic10(19).BackColor = Color.Black
        pic10(15).BackColor = Color.Black
        pic10(14).BackColor = Color.Black
        pic10(2).BackColor = Color.Black
        pic10(9).BackColor = Color.BlanchedAlmond
        pic10(10).BackColor = Color.BlanchedAlmond
        pic10(11).BackColor = Color.BlanchedAlmond
        pic10(12).BackColor = Color.BlanchedAlmond
        pic10(13).BackColor = Color.BlanchedAlmond
        pic10(16).BackColor = Color.BlanchedAlmond
        pic10(17).BackColor = Color.BlanchedAlmond
        pic10(18).BackColor = Color.BlanchedAlmond

        Dim pic11(20) As PictureBox
        For i = 0 To 19
            If pic11(i) Is Nothing Then
                pic11(i) = New PictureBox
                Me.Controls.Add(pic11(i))
            End If

            With pic11(i)
                .Enabled = True
                .Location = New Point(568, 56 + (i * 21))
                .Size = New Size(51, 20)
                .BackColor = Color.CadetBlue
                .BorderStyle = BorderStyle.FixedSingle
            End With
        Next
        pic11(0).Dispose()
        pic11(1).BackColor = Color.Black
        pic11(19).BackColor = Color.Black
        pic11(15).BackColor = Color.Black
        pic11(14).BackColor = Color.Black
        pic11(8).BackColor = Color.BlanchedAlmond
        pic11(9).BackColor = Color.BlanchedAlmond
        pic11(10).BackColor = Color.BlanchedAlmond
        pic11(11).BackColor = Color.BlanchedAlmond
        pic11(12).BackColor = Color.BlanchedAlmond
        pic11(13).BackColor = Color.BlanchedAlmond
        pic11(16).BackColor = Color.BlanchedAlmond
        pic11(17).BackColor = Color.BlanchedAlmond
        pic11(18).BackColor = Color.BlanchedAlmond

        Dim pic12(20) As PictureBox
        For i = 0 To 19
            If pic12(i) Is Nothing Then
                pic12(i) = New PictureBox
                Me.Controls.Add(pic12(i))
            End If

            With pic12(i)
                .Enabled = True
                .Location = New Point(620, 56 + (i * 21))
                .Size = New Size(51, 20)
                .BackColor = Color.CadetBlue
                .BorderStyle = BorderStyle.FixedSingle
            End With
        Next
        pic12(0).BackColor = Color.Black
        pic12(19).BackColor = Color.Black
        pic12(18).BackColor = Color.Black
        pic12(14).BackColor = Color.Black
        pic12(9).BackColor = Color.BlanchedAlmond
        pic12(10).BackColor = Color.BlanchedAlmond
        pic12(11).BackColor = Color.BlanchedAlmond
        pic12(12).BackColor = Color.BlanchedAlmond
        pic12(13).BackColor = Color.BlanchedAlmond
        pic12(16).BackColor = Color.Peru
        pic12(17).BackColor = Color.Peru

        Dim pic13(20) As PictureBox
        For i = 0 To 19
            If pic13(i) Is Nothing Then
                pic13(i) = New PictureBox
                Me.Controls.Add(pic13(i))
            End If

            With pic13(i)
                .Enabled = True
                .Location = New Point(672, 56 + (i * 21))
                .Size = New Size(51, 20)
                .BackColor = Color.CadetBlue
                .BorderStyle = BorderStyle.FixedSingle
            End With
        Next
        pic13(19).Dispose()
        pic13(0).BackColor = Color.Black
        pic13(18).BackColor = Color.Black
        pic13(10).BackColor = Color.BlanchedAlmond
        pic13(11).BackColor = Color.BlanchedAlmond
        pic13(12).BackColor = Color.BlanchedAlmond
        pic13(13).BackColor = Color.BlanchedAlmond
        pic13(14).BackColor = Color.BlanchedAlmond
        pic13(15).BackColor = Color.BlanchedAlmond
        pic13(16).BackColor = Color.Peru
        pic13(17).BackColor = Color.Peru

        Dim pic14(20) As PictureBox
        For i = 0 To 19
            If pic14(i) Is Nothing Then
                pic14(i) = New PictureBox
                Me.Controls.Add(pic14(i))
            End If

            With pic14(i)
                .Enabled = True
                .Location = New Point(724, 56 + (i * 21))
                .Size = New Size(51, 20)
                .BackColor = Color.Black
                .BorderStyle = BorderStyle.FixedSingle
            End With
        Next
        pic14(0).Dispose()
        pic14(19).Dispose()
        pic14(18).Dispose()
        pic14(11).BackColor = Color.BlanchedAlmond
        pic14(12).BackColor = Color.BlanchedAlmond
        pic14(13).BackColor = Color.BlanchedAlmond
        pic14(14).BackColor = Color.BlanchedAlmond
        pic14(15).BackColor = Color.BlanchedAlmond
        pic14(7).BackColor = Color.CadetBlue
        pic14(8).BackColor = Color.CadetBlue
        pic14(9).BackColor = Color.CadetBlue
        pic14(10).BackColor = Color.CadetBlue

        Dim pic15(9) As PictureBox
        For i = 0 To 8
            If pic15(i) Is Nothing Then
                pic15(i) = New PictureBox
                Me.Controls.Add(pic15(i))
            End If

            With pic15(i)
                .Enabled = True
                .Location = New Point(776, 203 + (i * 21))
                .Size = New Size(51, 20)
                .BackColor = Color.Black
                .BorderStyle = BorderStyle.FixedSingle
            End With
        Next

    End Sub


    Dim rand As New Random
    Function AAA()
        Dim LX As Integer
        Dim LY As Integer
        Dim style As Integer

        LX = rand.Next(0, 854)
        LY = rand.Next(500, 595)
        style = rand.Next(1, 5)

        If style = 1 Then
            Dim L As New PictureBox
            With L
                .Width = 30  '寬
                .Height = 30 '長
                .SizeMode = PictureBoxSizeMode.StretchImage
                .Image = My.Resources.shine
                .BorderStyle = BorderStyle.FixedSingle
                .Left = LX 'x座標
                .Top = LY 'y座標
                .Tag = 1
            End With
            Me.Controls.Add(L) '磚塊加入表單
        End If

        If style = 2 Then
            Dim L As New PictureBox
            With L
                .Width = 30  '寬
                .Height = 30 '長
                .SizeMode = PictureBoxSizeMode.StretchImage
                .Image = My.Resources.right
                .BorderStyle = BorderStyle.FixedSingle
                .Left = LX 'x座標
                .Top = LY 'y座標
                .Tag = 2
            End With
            Me.Controls.Add(L) '磚塊加入表單
        End If

        If style = 3 Then
            Dim L As New PictureBox
            With L
                .Width = 30  '寬
                .Height = 30 '長
                .SizeMode = PictureBoxSizeMode.StretchImage
                .Image = My.Resources.black
                .BorderStyle = BorderStyle.FixedSingle
                .Left = LX 'x座標
                .Top = LY 'y座標
                .Tag = 3
            End With
            Me.Controls.Add(L) '磚塊加入表單
        End If

        If style = 4 Then
            Dim L As New PictureBox
            With L
                .Width = 30  '寬
                .Height = 30 '長
                .SizeMode = PictureBoxSizeMode.StretchImage
                .Image = My.Resources.balls
                .BorderStyle = BorderStyle.FixedSingle
                .Left = LX 'x座標
                .Top = LY 'y座標
                .Tag = 4
            End With
            Me.Controls.Add(L) '磚塊加入表單
        End If

    End Function

    Private Sub Timer2_Tick(sender As Object, e As EventArgs) Handles Timer2.Tick
        AAA()
    End Sub
    Dim t As Integer = 0
    Dim n As Integer = 0
    '閃爍
    Private Sub Timer3_Tick(sender As Object, e As EventArgs) Handles Timer3.Tick
        n = n + 1

        If n Mod 2 = 0 Then
            For Each Ctl As Control In Me.Controls
                If TypeOf Ctl Is PictureBox Then
                    Ctl.Visible = True
                End If
            Next
        Else
            For Each Ctl As Control In Me.Controls
                If TypeOf Ctl Is PictureBox Then
                    Ctl.Visible = False
                End If
            Next
        End If
        If n >= 10 Then
            For Each Ctl As Control In Me.Controls
                If TypeOf Ctl Is PictureBox Then
                    Ctl.Visible = True
                End If
            Next
            n = 0
            Timer3.Enabled = False
        End If
    End Sub
    '黑頻
    Private Sub Timer4_Tick(sender As Object, e As EventArgs) Handles Timer4.Tick
        t = t + 1

        For Each Ctl As Control In Me.Controls
            If TypeOf Ctl Is PictureBox Then
                Ctl.Visible = False
            End If
        Next
        For Each Ctl As Control In Me.Controls
            If TypeOf Ctl Is Label Then
                Ctl.Visible = False
            End If
        Next

        If t >= 2 Then
            t = 0
            For Each Ctl As Control In Me.Controls
                If TypeOf Ctl Is PictureBox Then
                    Ctl.Visible = True
                End If
            Next
            For Each Ctl As Control In Me.Controls
                If TypeOf Ctl Is Label Then
                    Ctl.Visible = True
                End If
            Next
            Timer4.Enabled = False
        End If
    End Sub


    Private Sub Form2_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Me.Width = 900
        Me.Height = 685
        New_ball()
        Dim i As Integer
        Dim pic1(9) As PictureBox
        For i = 0 To 8
            If pic1(i) Is Nothing Then
                pic1(i) = New PictureBox
                Me.Controls.Add(pic1(i))
            End If

            With pic1(i)
                .Enabled = True
                .Location = New Point(48, 203 + (i * 21))
                .Size = New Size(51, 20)
                .BackColor = Color.Black
                .BorderStyle = BorderStyle.FixedSingle
            End With
        Next

        Dim pic2(20) As PictureBox
        For i = 0 To 19
            If pic2(i) Is Nothing Then
                pic2(i) = New PictureBox
                Me.Controls.Add(pic2(i))
            End If

            With pic2(i)
                .Enabled = True
                .Location = New Point(100, 56 + (i * 21))
                .Size = New Size(51, 20)
                .BackColor = Color.Black
                .BorderStyle = BorderStyle.FixedSingle
            End With
        Next
        pic2(0).Dispose()
        pic2(18).Dispose()
        pic2(19).Dispose()
        pic2(11).BackColor = Color.BlanchedAlmond
        pic2(12).BackColor = Color.BlanchedAlmond
        pic2(13).BackColor = Color.BlanchedAlmond
        pic2(14).BackColor = Color.BlanchedAlmond
        pic2(15).BackColor = Color.BlanchedAlmond
        pic2(7).BackColor = Color.CadetBlue
        pic2(8).BackColor = Color.CadetBlue
        pic2(9).BackColor = Color.CadetBlue
        pic2(10).BackColor = Color.CadetBlue

        Dim pic3(20) As PictureBox
        For i = 0 To 19
            If pic3(i) Is Nothing Then
                pic3(i) = New PictureBox
                Me.Controls.Add(pic3(i))
            End If

            With pic3(i)
                .Enabled = True
                .Location = New Point(152, 56 + (i * 21))
                .Size = New Size(51, 20)
                .BackColor = Color.CadetBlue
                .BorderStyle = BorderStyle.FixedSingle
            End With
        Next
        pic3(19).Dispose()
        pic3(0).BackColor = Color.Black
        pic3(18).BackColor = Color.Black
        pic3(10).BackColor = Color.BlanchedAlmond
        pic3(11).BackColor = Color.BlanchedAlmond
        pic3(12).BackColor = Color.BlanchedAlmond
        pic3(13).BackColor = Color.BlanchedAlmond
        pic3(14).BackColor = Color.BlanchedAlmond
        pic3(15).BackColor = Color.BlanchedAlmond
        pic3(16).BackColor = Color.Peru
        pic3(17).BackColor = Color.Peru

        Dim pic4(20) As PictureBox
        For i = 0 To 19
            If pic4(i) Is Nothing Then
                pic4(i) = New PictureBox
                Me.Controls.Add(pic4(i))
            End If

            With pic4(i)
                .Enabled = True
                .Location = New Point(204, 56 + (i * 21))
                .Size = New Size(51, 20)
                .BackColor = Color.CadetBlue
                .BorderStyle = BorderStyle.FixedSingle
            End With
        Next
        pic4(0).BackColor = Color.Black
        pic4(19).BackColor = Color.Black
        pic4(14).BackColor = Color.Black
        pic4(15).BackColor = Color.Black
        pic4(18).BackColor = Color.Black
        pic4(9).BackColor = Color.BlanchedAlmond
        pic4(10).BackColor = Color.BlanchedAlmond
        pic4(11).BackColor = Color.BlanchedAlmond
        pic4(12).BackColor = Color.BlanchedAlmond
        pic4(13).BackColor = Color.BlanchedAlmond
        pic4(16).BackColor = Color.Peru
        pic4(17).BackColor = Color.Peru

        Dim pic5(20) As PictureBox
        For i = 0 To 19
            If pic5(i) Is Nothing Then
                pic5(i) = New PictureBox
                Me.Controls.Add(pic5(i))
            End If

            With pic5(i)
                .Enabled = True
                .Location = New Point(256, 56 + (i * 21))
                .Size = New Size(51, 20)
                .BackColor = Color.CadetBlue
                .BorderStyle = BorderStyle.FixedSingle
            End With
        Next
        pic5(0).Dispose()
        pic5(1).BackColor = Color.Black
        pic5(19).BackColor = Color.Black
        pic5(15).BackColor = Color.Black
        pic5(14).BackColor = Color.Black
        pic5(8).BackColor = Color.BlanchedAlmond
        pic5(9).BackColor = Color.BlanchedAlmond
        pic5(10).BackColor = Color.BlanchedAlmond
        pic5(11).BackColor = Color.BlanchedAlmond
        pic5(12).BackColor = Color.BlanchedAlmond
        pic5(13).BackColor = Color.BlanchedAlmond
        pic5(16).BackColor = Color.BlanchedAlmond
        pic5(17).BackColor = Color.BlanchedAlmond
        pic5(18).BackColor = Color.BlanchedAlmond

        Dim pic6(20) As PictureBox
        For i = 0 To 19
            If pic6(i) Is Nothing Then
                pic6(i) = New PictureBox
                Me.Controls.Add(pic6(i))
            End If

            With pic6(i)
                .Enabled = True
                .Location = New Point(308, 56 + (i * 21))
                .Size = New Size(51, 20)
                .BackColor = Color.CadetBlue
                .BorderStyle = BorderStyle.FixedSingle
            End With
        Next
        pic6(0).Dispose()
        pic6(1).BackColor = Color.Black
        pic6(19).BackColor = Color.Black
        pic6(15).BackColor = Color.Black
        pic6(14).BackColor = Color.Black
        pic6(2).BackColor = Color.Black
        pic6(9).BackColor = Color.BlanchedAlmond
        pic6(10).BackColor = Color.BlanchedAlmond
        pic6(11).BackColor = Color.BlanchedAlmond
        pic6(12).BackColor = Color.BlanchedAlmond
        pic6(13).BackColor = Color.BlanchedAlmond
        pic6(16).BackColor = Color.BlanchedAlmond
        pic6(17).BackColor = Color.BlanchedAlmond
        pic6(18).BackColor = Color.BlanchedAlmond

        Dim pic7(20) As PictureBox
        For i = 0 To 19
            If pic7(i) Is Nothing Then
                pic7(i) = New PictureBox
                Me.Controls.Add(pic7(i))
            End If

            With pic7(i)
                .Enabled = True
                .Location = New Point(360, 56 + (i * 21))
                .Size = New Size(51, 20)
                .BackColor = Color.CadetBlue
                .BorderStyle = BorderStyle.FixedSingle
            End With
        Next
        pic7(0).Dispose()
        pic7(1).BackColor = Color.Black
        pic7(19).BackColor = Color.Black
        pic7(10).BackColor = Color.BlanchedAlmond
        pic7(11).BackColor = Color.BlanchedAlmond
        pic7(12).BackColor = Color.BlanchedAlmond
        pic7(13).BackColor = Color.BlanchedAlmond
        pic7(14).BackColor = Color.BlanchedAlmond
        pic7(15).BackColor = Color.BlanchedAlmond
        pic7(16).BackColor = Color.BlanchedAlmond
        pic7(17).BackColor = Color.BlanchedAlmond
        pic7(18).BackColor = Color.BlanchedAlmond

        Dim pic8(20) As PictureBox
        For i = 0 To 19
            If pic8(i) Is Nothing Then
                pic8(i) = New PictureBox
                Me.Controls.Add(pic8(i))
            End If

            With pic8(i)
                .Enabled = True
                .Location = New Point(412, 56 + (i * 21))
                .Size = New Size(51, 20)
                .BackColor = Color.CadetBlue
                .BorderStyle = BorderStyle.FixedSingle
            End With
        Next
        pic8(0).Dispose()
        pic8(1).BackColor = Color.Black
        pic8(19).BackColor = Color.Black
        pic8(11).BackColor = Color.BlanchedAlmond
        pic8(12).BackColor = Color.BlanchedAlmond
        pic8(13).BackColor = Color.BlanchedAlmond
        pic8(14).BackColor = Color.BlanchedAlmond
        pic8(15).BackColor = Color.BlanchedAlmond
        pic8(16).BackColor = Color.BlanchedAlmond
        pic8(17).BackColor = Color.BlanchedAlmond
        pic8(18).BackColor = Color.BlanchedAlmond

        Dim pic9(20) As PictureBox
        For i = 0 To 19
            If pic9(i) Is Nothing Then
                pic9(i) = New PictureBox
                Me.Controls.Add(pic9(i))
            End If

            With pic9(i)
                .Enabled = True
                .Location = New Point(464, 56 + (i * 21))
                .Size = New Size(51, 20)
                .BackColor = Color.CadetBlue
                .BorderStyle = BorderStyle.FixedSingle
            End With
        Next
        pic9(0).Dispose()
        pic9(1).BackColor = Color.Black
        pic9(19).BackColor = Color.Black
        pic9(10).BackColor = Color.BlanchedAlmond
        pic9(11).BackColor = Color.BlanchedAlmond
        pic9(12).BackColor = Color.BlanchedAlmond
        pic9(13).BackColor = Color.BlanchedAlmond
        pic9(14).BackColor = Color.BlanchedAlmond
        pic9(15).BackColor = Color.BlanchedAlmond
        pic9(16).BackColor = Color.BlanchedAlmond
        pic9(17).BackColor = Color.BlanchedAlmond
        pic9(18).BackColor = Color.BlanchedAlmond

        Dim pic10(20) As PictureBox
        For i = 0 To 19
            If pic10(i) Is Nothing Then
                pic10(i) = New PictureBox
                Me.Controls.Add(pic10(i))
            End If

            With pic10(i)
                .Enabled = True
                .Location = New Point(516, 56 + (i * 21))
                .Size = New Size(51, 20)
                .BackColor = Color.CadetBlue
                .BorderStyle = BorderStyle.FixedSingle
            End With
        Next
        pic10(0).Dispose()
        pic10(1).BackColor = Color.Black
        pic10(19).BackColor = Color.Black
        pic10(15).BackColor = Color.Black
        pic10(14).BackColor = Color.Black
        pic10(2).BackColor = Color.Black
        pic10(9).BackColor = Color.BlanchedAlmond
        pic10(10).BackColor = Color.BlanchedAlmond
        pic10(11).BackColor = Color.BlanchedAlmond
        pic10(12).BackColor = Color.BlanchedAlmond
        pic10(13).BackColor = Color.BlanchedAlmond
        pic10(16).BackColor = Color.BlanchedAlmond
        pic10(17).BackColor = Color.BlanchedAlmond
        pic10(18).BackColor = Color.BlanchedAlmond

        Dim pic11(20) As PictureBox
        For i = 0 To 19
            If pic11(i) Is Nothing Then
                pic11(i) = New PictureBox
                Me.Controls.Add(pic11(i))
            End If

            With pic11(i)
                .Enabled = True
                .Location = New Point(568, 56 + (i * 21))
                .Size = New Size(51, 20)
                .BackColor = Color.CadetBlue
                .BorderStyle = BorderStyle.FixedSingle
            End With
        Next
        pic11(0).Dispose()
        pic11(1).BackColor = Color.Black
        pic11(19).BackColor = Color.Black
        pic11(15).BackColor = Color.Black
        pic11(14).BackColor = Color.Black
        pic11(8).BackColor = Color.BlanchedAlmond
        pic11(9).BackColor = Color.BlanchedAlmond
        pic11(10).BackColor = Color.BlanchedAlmond
        pic11(11).BackColor = Color.BlanchedAlmond
        pic11(12).BackColor = Color.BlanchedAlmond
        pic11(13).BackColor = Color.BlanchedAlmond
        pic11(16).BackColor = Color.BlanchedAlmond
        pic11(17).BackColor = Color.BlanchedAlmond
        pic11(18).BackColor = Color.BlanchedAlmond

        Dim pic12(20) As PictureBox
        For i = 0 To 19
            If pic12(i) Is Nothing Then
                pic12(i) = New PictureBox
                Me.Controls.Add(pic12(i))
            End If

            With pic12(i)
                .Enabled = True
                .Location = New Point(620, 56 + (i * 21))
                .Size = New Size(51, 20)
                .BackColor = Color.CadetBlue
                .BorderStyle = BorderStyle.FixedSingle
            End With
        Next
        pic12(0).BackColor = Color.Black
        pic12(19).BackColor = Color.Black
        pic12(18).BackColor = Color.Black
        pic12(14).BackColor = Color.Black
        pic12(15).BackColor = Color.Black
        pic12(9).BackColor = Color.BlanchedAlmond
        pic12(10).BackColor = Color.BlanchedAlmond
        pic12(11).BackColor = Color.BlanchedAlmond
        pic12(12).BackColor = Color.BlanchedAlmond
        pic12(13).BackColor = Color.BlanchedAlmond
        pic12(16).BackColor = Color.Peru
        pic12(17).BackColor = Color.Peru

        Dim pic13(20) As PictureBox
        For i = 0 To 19
            If pic13(i) Is Nothing Then
                pic13(i) = New PictureBox
                Me.Controls.Add(pic13(i))
            End If

            With pic13(i)
                .Enabled = True
                .Location = New Point(672, 56 + (i * 21))
                .Size = New Size(51, 20)
                .BackColor = Color.CadetBlue
                .BorderStyle = BorderStyle.FixedSingle
            End With
        Next
        pic13(19).Dispose()
        pic13(0).BackColor = Color.Black
        pic13(18).BackColor = Color.Black
        pic13(10).BackColor = Color.BlanchedAlmond
        pic13(11).BackColor = Color.BlanchedAlmond
        pic13(12).BackColor = Color.BlanchedAlmond
        pic13(13).BackColor = Color.BlanchedAlmond
        pic13(14).BackColor = Color.BlanchedAlmond
        pic13(15).BackColor = Color.BlanchedAlmond
        pic13(16).BackColor = Color.Peru
        pic13(17).BackColor = Color.Peru

        Dim pic14(20) As PictureBox
        For i = 0 To 19
            If pic14(i) Is Nothing Then
                pic14(i) = New PictureBox
                Me.Controls.Add(pic14(i))
            End If

            With pic14(i)
                .Enabled = True
                .Location = New Point(724, 56 + (i * 21))
                .Size = New Size(51, 20)
                .BackColor = Color.Black
                .BorderStyle = BorderStyle.FixedSingle
            End With
        Next
        pic14(0).Dispose()
        pic14(19).Dispose()
        pic14(18).Dispose()
        pic14(11).BackColor = Color.BlanchedAlmond
        pic14(12).BackColor = Color.BlanchedAlmond
        pic14(13).BackColor = Color.BlanchedAlmond
        pic14(14).BackColor = Color.BlanchedAlmond
        pic14(15).BackColor = Color.BlanchedAlmond
        pic14(7).BackColor = Color.CadetBlue
        pic14(8).BackColor = Color.CadetBlue
        pic14(9).BackColor = Color.CadetBlue
        pic14(10).BackColor = Color.CadetBlue

        Dim pic15(9) As PictureBox
        For i = 0 To 8
            If pic15(i) Is Nothing Then
                pic15(i) = New PictureBox
                Me.Controls.Add(pic15(i))
            End If

            With pic15(i)
                .Enabled = True
                .Location = New Point(776, 203 + (i * 21))
                .Size = New Size(51, 20)
                .BackColor = Color.Black
                .BorderStyle = BorderStyle.FixedSingle
            End With
        Next

    End Sub
End Class