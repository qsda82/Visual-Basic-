Public Class L2P2
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
            P.Top = 629 '調整球拍的y座標
            P.Left = (Me.ClientSize.Width - P.Width) / 2 '球拍水平置中
            spacecheck = True
            New_ball()
        ElseIf Dietimes = 2 Then
            Label6.Image = Nothing
            Label5.Image = Nothing
            MsgBox("你死了，還剩1次機會")
            P.Top = 629 '調整球拍的y座標
            P.Left = (Me.ClientSize.Width - P.Width) / 2 '球拍水平置中
            spacecheck = True
            New_ball()
        ElseIf Dietimes = 3 Then
            Label6.Image = Nothing
            Label5.Image = Nothing
            Label4.Image = Nothing
            YOUDIE.Button1.Tag = 5
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

            If check = 273 Then

                Timer1.Stop()
                PASS.Button1.Tag = 5
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
            .Location = New Point(P.Location.X + P.Size.Width / 2 - 15, 600)
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
        P.Top = 629 '調整球拍的y座標
        P.Left = (Me.ClientSize.Width - P.Width) / 2 '球拍水平置中
        P.Width = 131
        spacecheck = True
        New_ball()
        Dim i As Integer
        Dim pic1(13) As PictureBox
        For i = 0 To 12
            If pic1(i) Is Nothing Then
                pic1(i) = New PictureBox
                Me.Controls.Add(pic1(i))
            End If

            With pic1(i)
                .Enabled = True
                .Location = New Point(28, 198 + (i * 21))
                .Size = New Size(51, 20)
                .BackColor = Color.SeaGreen
                .BorderStyle = BorderStyle.FixedSingle
            End With
        Next

        Dim pic2(15) As PictureBox
        For i = 0 To 14
            If pic2(i) Is Nothing Then
                pic2(i) = New PictureBox
                Me.Controls.Add(pic2(i))
            End If

            With pic2(i)
                .Enabled = True
                .Location = New Point(80, 177 + (i * 21))
                .Size = New Size(51, 20)
                .BackColor = Color.LimeGreen
                .BorderStyle = BorderStyle.FixedSingle
            End With
        Next
        pic2(0).BackColor = Color.SeaGreen
        pic2(14).BackColor = Color.SeaGreen

        Dim pic3(23) As PictureBox
        For i = 0 To 22
            If pic3(i) Is Nothing Then
                pic3(i) = New PictureBox
                Me.Controls.Add(pic3(i))
            End If

            With pic3(i)
                .Enabled = True
                .Location = New Point(132, 51 + (i * 21))
                .Size = New Size(51, 20)
                .BackColor = Color.LimeGreen
                .BorderStyle = BorderStyle.FixedSingle
            End With
        Next
        pic3(0).Dispose()
        pic3(1).Dispose()
        pic3(2).Dispose()
        pic3(4).BackColor = Color.SeaGreen
        pic3(22).BackColor = Color.SeaGreen
        pic3(21).BackColor = Color.SeaGreen
        pic3(18).BackColor = Color.SeaGreen

        Dim pic4(23) As PictureBox
        For i = 0 To 22
            If pic4(i) Is Nothing Then
                pic4(i) = New PictureBox
                Me.Controls.Add(pic4(i))
            End If

            With pic4(i)
                .Enabled = True
                .Location = New Point(184, 51 + (i * 21))
                .Size = New Size(51, 20)
                .BackColor = Color.LimeGreen
                .BorderStyle = BorderStyle.FixedSingle
            End With
        Next
        pic4(0).Dispose()
        pic4(22).BackColor = Color.SeaGreen
        pic4(19).BackColor = Color.SeaGreen
        pic4(1).BackColor = Color.SeaGreen
        pic4(7).BackColor = Color.White
        pic4(15).BackColor = Color.Coral
        pic4(16).BackColor = Color.Coral


        Dim pic5(23) As PictureBox
        For i = 0 To 22
            If pic5(i) Is Nothing Then
                pic5(i) = New PictureBox
                Me.Controls.Add(pic5(i))
            End If

            With pic5(i)
                .Enabled = True
                .Location = New Point(236, 51 + (i * 21))
                .Size = New Size(51, 20)
                .BackColor = Color.LimeGreen
                .BorderStyle = BorderStyle.FixedSingle
            End With
        Next
        pic5(5).BackColor = Color.SeaGreen
        pic5(22).BackColor = Color.SeaGreen
        pic5(20).BackColor = Color.SeaGreen
        pic5(2).BackColor = Color.SeaGreen
        pic5(7).BackColor = Color.White
        pic5(8).BackColor = Color.White
        pic5(14).BackColor = Color.Coral
        pic5(16).BackColor = Color.Coral
        pic5(17).BackColor = Color.Coral
        pic5(15).BackColor = Color.IndianRed

        Dim pic6(23) As PictureBox
        For i = 0 To 22
            If pic6(i) Is Nothing Then
                pic6(i) = New PictureBox
                Me.Controls.Add(pic6(i))
            End If

            With pic6(i)
                .Enabled = True
                .Location = New Point(288, 51 + (i * 21))
                .Size = New Size(51, 20)
                .BackColor = Color.LimeGreen
                .BorderStyle = BorderStyle.FixedSingle
            End With
        Next
        pic6(22).BackColor = Color.SeaGreen
        pic6(4).BackColor = Color.SeaGreen
        pic6(2).BackColor = Color.SeaGreen
        pic6(6).BackColor = Color.White
        pic6(7).BackColor = Color.Black
        pic6(8).BackColor = Color.Black
        pic6(14).BackColor = Color.Coral
        pic6(15).BackColor = Color.Coral
        pic6(17).BackColor = Color.Coral
        pic6(16).BackColor = Color.IndianRed

        Dim pic7(23) As PictureBox
        For i = 0 To 22
            If pic7(i) Is Nothing Then
                pic7(i) = New PictureBox
                Me.Controls.Add(pic7(i))
            End If

            With pic7(i)
                .Enabled = True
                .Location = New Point(340, 51 + (i * 21))
                .Size = New Size(51, 20)
                .BackColor = Color.LimeGreen
                .BorderStyle = BorderStyle.FixedSingle
            End With
        Next
        pic7(4).BackColor = Color.SeaGreen
        pic7(2).BackColor = Color.SeaGreen
        pic7(22).BackColor = Color.SeaGreen
        pic7(10).BackColor = Color.SeaGreen
        pic7(6).BackColor = Color.Black
        pic7(8).BackColor = Color.Black
        pic7(7).BackColor = Color.Black
        pic7(15).BackColor = Color.Coral
        pic7(17).BackColor = Color.Coral
        pic7(16).BackColor = Color.IndianRed

        Dim pic8(23) As PictureBox
        For i = 0 To 22
            If pic8(i) Is Nothing Then
                pic8(i) = New PictureBox
                Me.Controls.Add(pic8(i))
            End If

            With pic8(i)
                .Enabled = True
                .Location = New Point(392, 51 + (i * 21))
                .Size = New Size(51, 20)
                .BackColor = Color.LimeGreen
                .BorderStyle = BorderStyle.FixedSingle
            End With
        Next
        pic8(0).BackColor = Color.SeaGreen
        pic8(1).BackColor = Color.SeaGreen
        pic8(2).BackColor = Color.SeaGreen
        pic8(4).BackColor = Color.SeaGreen
        pic8(9).BackColor = Color.SeaGreen
        pic8(10).BackColor = Color.SeaGreen
        pic8(22).BackColor = Color.SeaGreen
        pic8(6).BackColor = Color.White
        pic8(7).BackColor = Color.Black
        pic8(8).BackColor = Color.Black
        pic8(15).BackColor = Color.Coral
        pic8(17).BackColor = Color.Coral
        pic8(16).BackColor = Color.IndianRed

        Dim pic9(23) As PictureBox
        For i = 0 To 22
            If pic9(i) Is Nothing Then
                pic9(i) = New PictureBox
                Me.Controls.Add(pic9(i))
            End If

            With pic9(i)
                .Enabled = True
                .Location = New Point(444, 51 + (i * 21))
                .Size = New Size(51, 20)
                .BackColor = Color.LimeGreen
                .BorderStyle = BorderStyle.FixedSingle
            End With
        Next
        pic9(2).BackColor = Color.SeaGreen
        pic9(5).BackColor = Color.SeaGreen
        pic9(8).BackColor = Color.SeaGreen
        pic9(22).BackColor = Color.SeaGreen
        pic9(7).BackColor = Color.White
        pic9(15).BackColor = Color.Coral
        pic9(17).BackColor = Color.Coral
        pic9(16).BackColor = Color.IndianRed

        Dim pic10(23) As PictureBox
        For i = 0 To 22
            If pic10(i) Is Nothing Then
                pic10(i) = New PictureBox
                Me.Controls.Add(pic10(i))
            End If

            With pic10(i)
                .Enabled = True
                .Location = New Point(496, 51 + (i * 21))
                .Size = New Size(51, 20)
                .BackColor = Color.LimeGreen
                .BorderStyle = BorderStyle.FixedSingle
            End With
        Next
        pic10(1).BackColor = Color.SeaGreen
        pic10(5).BackColor = Color.SeaGreen
        pic10(22).BackColor = Color.SeaGreen
        pic10(7).BackColor = Color.White
        pic10(8).BackColor = Color.White
        pic10(15).BackColor = Color.Coral
        pic10(17).BackColor = Color.Coral
        pic10(16).BackColor = Color.IndianRed

        Dim pic11(23) As PictureBox
        For i = 0 To 22
            If pic11(i) Is Nothing Then
                pic11(i) = New PictureBox
                Me.Controls.Add(pic11(i))
            End If

            With pic11(i)
                .Enabled = True
                .Location = New Point(548, 51 + (i * 21))
                .Size = New Size(51, 20)
                .BackColor = Color.LimeGreen
                .BorderStyle = BorderStyle.FixedSingle
            End With
        Next
        pic11(2).BackColor = Color.SeaGreen
        pic11(4).BackColor = Color.SeaGreen
        pic11(9).BackColor = Color.SeaGreen
        pic11(22).BackColor = Color.SeaGreen
        pic11(6).BackColor = Color.White
        pic11(7).BackColor = Color.Black
        pic11(8).BackColor = Color.Black
        pic11(15).BackColor = Color.Coral
        pic11(17).BackColor = Color.Coral
        pic11(16).BackColor = Color.IndianRed

        Dim pic12(23) As PictureBox
        For i = 0 To 22
            If pic12(i) Is Nothing Then
                pic12(i) = New PictureBox
                Me.Controls.Add(pic12(i))
            End If

            With pic12(i)
                .Enabled = True
                .Location = New Point(600, 51 + (i * 21))
                .Size = New Size(51, 20)
                .BackColor = Color.LimeGreen
                .BorderStyle = BorderStyle.FixedSingle
            End With
        Next
        pic12(0).Dispose()
        pic12(1).Dispose()
        pic12(22).Dispose()
        pic12(4).BackColor = Color.SeaGreen
        pic12(10).BackColor = Color.SeaGreen
        pic12(21).BackColor = Color.SeaGreen
        pic12(6).BackColor = Color.Black
        pic12(8).BackColor = Color.Black
        pic12(7).BackColor = Color.Black
        pic12(15).BackColor = Color.Coral
        pic12(17).BackColor = Color.Coral
        pic12(16).BackColor = Color.IndianRed

        Dim pic13(23) As PictureBox
        For i = 0 To 22
            If pic13(i) Is Nothing Then
                pic13(i) = New PictureBox
                Me.Controls.Add(pic13(i))
            End If

            With pic13(i)
                .Enabled = True
                .Location = New Point(652, 51 + (i * 21))
                .Size = New Size(51, 20)
                .BackColor = Color.LimeGreen
                .BorderStyle = BorderStyle.FixedSingle
            End With
        Next
        pic13(0).Dispose()
        pic13(1).Dispose()
        pic13(2).Dispose()
        pic13(10).Dispose()
        pic13(11).Dispose()
        pic13(21).Dispose()
        pic13(22).Dispose()
        pic13(5).BackColor = Color.SeaGreen
        pic13(19).BackColor = Color.SeaGreen
        pic13(20).BackColor = Color.SeaGreen
        pic13(6).BackColor = Color.White
        pic13(7).BackColor = Color.Black
        pic13(8).BackColor = Color.Black
        pic13(15).BackColor = Color.Coral
        pic13(17).BackColor = Color.Coral
        pic13(16).BackColor = Color.IndianRed

        Dim pic14(11) As PictureBox
        For i = 0 To 10
            If pic14(i) Is Nothing Then
                pic14(i) = New PictureBox
                Me.Controls.Add(pic14(i))
            End If

            With pic14(i)
                .Enabled = True
                .Location = New Point(704, 156 + (i * 21))
                .Size = New Size(51, 20)
                .BackColor = Color.LimeGreen
                .BorderStyle = BorderStyle.FixedSingle
            End With
        Next
        pic14(4).Dispose()
        pic14(5).Dispose()
        pic14(6).Dispose()
        pic14(7).Dispose()
        pic14(8).Dispose()
        pic14(0).BackColor = Color.SeaGreen
        pic14(1).BackColor = Color.White
        pic14(2).BackColor = Color.White
        pic14(9).BackColor = Color.Coral
        pic14(10).BackColor = Color.Coral

    End Sub


    Dim rand As New Random
    Function AAA()
        Dim LX As Integer
        Dim LY As Integer
        Dim style As Integer

        LX = rand.Next(0, 753)
        LY = rand.Next(585, 600)
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

    Dim n As Integer = 0
    Dim t As Integer = 0
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


    Private Sub Form6_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Me.Width = 800
        Me.Height = 700
        New_ball()

        Dim i As Integer
        Dim pic1(13) As PictureBox
        For i = 0 To 12
            If pic1(i) Is Nothing Then
                pic1(i) = New PictureBox
                Me.Controls.Add(pic1(i))
            End If

            With pic1(i)
                .Enabled = True
                .Location = New Point(28, 198 + (i * 21))
                .Size = New Size(51, 20)
                .BackColor = Color.SeaGreen
                .BorderStyle = BorderStyle.FixedSingle
            End With
        Next

        Dim pic2(15) As PictureBox
        For i = 0 To 14
            If pic2(i) Is Nothing Then
                pic2(i) = New PictureBox
                Me.Controls.Add(pic2(i))
            End If

            With pic2(i)
                .Enabled = True
                .Location = New Point(80, 177 + (i * 21))
                .Size = New Size(51, 20)
                .BackColor = Color.LimeGreen
                .BorderStyle = BorderStyle.FixedSingle
            End With
        Next
        pic2(0).BackColor = Color.SeaGreen
        pic2(14).BackColor = Color.SeaGreen

        Dim pic3(23) As PictureBox
        For i = 0 To 22
            If pic3(i) Is Nothing Then
                pic3(i) = New PictureBox
                Me.Controls.Add(pic3(i))
            End If

            With pic3(i)
                .Enabled = True
                .Location = New Point(132, 51 + (i * 21))
                .Size = New Size(51, 20)
                .BackColor = Color.LimeGreen
                .BorderStyle = BorderStyle.FixedSingle
            End With
        Next
        pic3(0).Dispose()
        pic3(1).Dispose()
        pic3(2).Dispose()
        pic3(4).BackColor = Color.SeaGreen
        pic3(22).BackColor = Color.SeaGreen
        pic3(21).BackColor = Color.SeaGreen
        pic3(18).BackColor = Color.SeaGreen

        Dim pic4(23) As PictureBox
        For i = 0 To 22
            If pic4(i) Is Nothing Then
                pic4(i) = New PictureBox
                Me.Controls.Add(pic4(i))
            End If

            With pic4(i)
                .Enabled = True
                .Location = New Point(184, 51 + (i * 21))
                .Size = New Size(51, 20)
                .BackColor = Color.LimeGreen
                .BorderStyle = BorderStyle.FixedSingle
            End With
        Next
        pic4(0).Dispose()
        pic4(22).BackColor = Color.SeaGreen
        pic4(19).BackColor = Color.SeaGreen
        pic4(1).BackColor = Color.SeaGreen
        pic4(7).BackColor = Color.White
        pic4(15).BackColor = Color.Coral
        pic4(16).BackColor = Color.Coral


        Dim pic5(23) As PictureBox
        For i = 0 To 22
            If pic5(i) Is Nothing Then
                pic5(i) = New PictureBox
                Me.Controls.Add(pic5(i))
            End If

            With pic5(i)
                .Enabled = True
                .Location = New Point(236, 51 + (i * 21))
                .Size = New Size(51, 20)
                .BackColor = Color.LimeGreen

                .BorderStyle = BorderStyle.FixedSingle
            End With
        Next
        pic5(5).BackColor = Color.SeaGreen
        pic5(22).BackColor = Color.SeaGreen
        pic5(20).BackColor = Color.SeaGreen
        pic5(2).BackColor = Color.SeaGreen
        pic5(7).BackColor = Color.White
        pic5(8).BackColor = Color.White
        pic5(14).BackColor = Color.Coral
        pic5(16).BackColor = Color.Coral
        pic5(17).BackColor = Color.Coral
        pic5(15).BackColor = Color.IndianRed

        Dim pic6(23) As PictureBox
        For i = 0 To 22
            If pic6(i) Is Nothing Then
                pic6(i) = New PictureBox
                Me.Controls.Add(pic6(i))
            End If

            With pic6(i)
                .Enabled = True
                .Location = New Point(288, 51 + (i * 21))
                .Size = New Size(51, 20)
                .BackColor = Color.LimeGreen
                .BorderStyle = BorderStyle.FixedSingle
            End With
        Next
        pic6(22).BackColor = Color.SeaGreen
        pic6(4).BackColor = Color.SeaGreen
        pic6(2).BackColor = Color.SeaGreen
        pic6(6).BackColor = Color.White
        pic6(7).BackColor = Color.Black
        pic6(8).BackColor = Color.Black
        pic6(14).BackColor = Color.Coral
        pic6(15).BackColor = Color.Coral
        pic6(17).BackColor = Color.Coral
        pic6(16).BackColor = Color.IndianRed

        Dim pic7(23) As PictureBox
        For i = 0 To 22
            If pic7(i) Is Nothing Then
                pic7(i) = New PictureBox
                Me.Controls.Add(pic7(i))
            End If

            With pic7(i)
                .Enabled = True
                .Location = New Point(340, 51 + (i * 21))
                .Size = New Size(51, 20)
                .BackColor = Color.LimeGreen
                .BorderStyle = BorderStyle.FixedSingle
            End With
        Next
        pic7(4).BackColor = Color.SeaGreen
        pic7(2).BackColor = Color.SeaGreen
        pic7(22).BackColor = Color.SeaGreen
        pic7(10).BackColor = Color.SeaGreen
        pic7(6).BackColor = Color.Black
        pic7(8).BackColor = Color.Black
        pic7(7).BackColor = Color.Black
        pic7(15).BackColor = Color.Coral
        pic7(17).BackColor = Color.Coral
        pic7(16).BackColor = Color.IndianRed

        Dim pic8(23) As PictureBox
        For i = 0 To 22
            If pic8(i) Is Nothing Then
                pic8(i) = New PictureBox
                Me.Controls.Add(pic8(i))
            End If

            With pic8(i)
                .Enabled = True
                .Location = New Point(392, 51 + (i * 21))
                .Size = New Size(51, 20)
                .BackColor = Color.LimeGreen
                .BorderStyle = BorderStyle.FixedSingle
            End With
        Next
        pic8(0).BackColor = Color.SeaGreen
        pic8(1).BackColor = Color.SeaGreen
        pic8(2).BackColor = Color.SeaGreen
        pic8(4).BackColor = Color.SeaGreen
        pic8(9).BackColor = Color.SeaGreen
        pic8(10).BackColor = Color.SeaGreen
        pic8(22).BackColor = Color.SeaGreen
        pic8(6).BackColor = Color.White
        pic8(7).BackColor = Color.Black
        pic8(8).BackColor = Color.Black
        pic8(15).BackColor = Color.Coral
        pic8(17).BackColor = Color.Coral
        pic8(16).BackColor = Color.IndianRed

        Dim pic9(23) As PictureBox
        For i = 0 To 22
            If pic9(i) Is Nothing Then
                pic9(i) = New PictureBox
                Me.Controls.Add(pic9(i))
            End If

            With pic9(i)
                .Enabled = True
                .Location = New Point(444, 51 + (i * 21))
                .Size = New Size(51, 20)
                .BackColor = Color.LimeGreen
                .BorderStyle = BorderStyle.FixedSingle
            End With
        Next
        pic9(2).BackColor = Color.SeaGreen
        pic9(5).BackColor = Color.SeaGreen
        pic9(8).BackColor = Color.SeaGreen
        pic9(22).BackColor = Color.SeaGreen
        pic9(7).BackColor = Color.White
        pic9(15).BackColor = Color.Coral
        pic9(17).BackColor = Color.Coral
        pic9(16).BackColor = Color.IndianRed

        Dim pic10(23) As PictureBox
        For i = 0 To 22
            If pic10(i) Is Nothing Then
                pic10(i) = New PictureBox
                Me.Controls.Add(pic10(i))
            End If

            With pic10(i)
                .Enabled = True
                .Location = New Point(496, 51 + (i * 21))
                .Size = New Size(51, 20)
                .BackColor = Color.LimeGreen
                .BorderStyle = BorderStyle.FixedSingle
            End With
        Next
        pic10(1).BackColor = Color.SeaGreen
        pic10(5).BackColor = Color.SeaGreen
        pic10(22).BackColor = Color.SeaGreen
        pic10(7).BackColor = Color.White
        pic10(8).BackColor = Color.White
        pic10(15).BackColor = Color.Coral
        pic10(17).BackColor = Color.Coral
        pic10(16).BackColor = Color.IndianRed

        Dim pic11(23) As PictureBox
        For i = 0 To 22
            If pic11(i) Is Nothing Then
                pic11(i) = New PictureBox
                Me.Controls.Add(pic11(i))
            End If

            With pic11(i)
                .Enabled = True
                .Location = New Point(548, 51 + (i * 21))
                .Size = New Size(51, 20)
                .BackColor = Color.LimeGreen
                .BorderStyle = BorderStyle.FixedSingle
            End With
        Next
        pic11(2).BackColor = Color.SeaGreen
        pic11(4).BackColor = Color.SeaGreen
        pic11(9).BackColor = Color.SeaGreen
        pic11(22).BackColor = Color.SeaGreen
        pic11(6).BackColor = Color.White
        pic11(7).BackColor = Color.Black
        pic11(8).BackColor = Color.Black
        pic11(15).BackColor = Color.Coral
        pic11(17).BackColor = Color.Coral
        pic11(16).BackColor = Color.IndianRed

        Dim pic12(23) As PictureBox
        For i = 0 To 22
            If pic12(i) Is Nothing Then
                pic12(i) = New PictureBox
                Me.Controls.Add(pic12(i))
            End If

            With pic12(i)
                .Enabled = True
                .Location = New Point(600, 51 + (i * 21))
                .Size = New Size(51, 20)
                .BackColor = Color.LimeGreen
                .BorderStyle = BorderStyle.FixedSingle
            End With
        Next
        pic12(0).Dispose()
        pic12(1).Dispose()
        pic12(22).Dispose()
        pic12(4).BackColor = Color.SeaGreen
        pic12(10).BackColor = Color.SeaGreen
        pic12(21).BackColor = Color.SeaGreen
        pic12(6).BackColor = Color.Black
        pic12(8).BackColor = Color.Black
        pic12(7).BackColor = Color.Black
        pic12(15).BackColor = Color.Coral
        pic12(17).BackColor = Color.Coral
        pic12(16).BackColor = Color.IndianRed

        Dim pic13(23) As PictureBox
        For i = 0 To 22
            If pic13(i) Is Nothing Then
                pic13(i) = New PictureBox
                Me.Controls.Add(pic13(i))
            End If

            With pic13(i)
                .Enabled = True
                .Location = New Point(652, 51 + (i * 21))
                .Size = New Size(51, 20)
                .BackColor = Color.LimeGreen
                .BorderStyle = BorderStyle.FixedSingle
            End With
        Next
        pic13(0).Dispose()
        pic13(1).Dispose()
        pic13(2).Dispose()
        pic13(10).Dispose()
        pic13(11).Dispose()
        pic13(21).Dispose()
        pic13(22).Dispose()
        pic13(5).BackColor = Color.SeaGreen
        pic13(19).BackColor = Color.SeaGreen
        pic13(20).BackColor = Color.SeaGreen
        pic13(6).BackColor = Color.White
        pic13(7).BackColor = Color.Black
        pic13(8).BackColor = Color.Black
        pic13(15).BackColor = Color.Coral
        pic13(17).BackColor = Color.Coral
        pic13(16).BackColor = Color.IndianRed

        Dim pic14(11) As PictureBox
        For i = 0 To 10
            If pic14(i) Is Nothing Then
                pic14(i) = New PictureBox
                Me.Controls.Add(pic14(i))
            End If

            With pic14(i)
                .Enabled = True
                .Location = New Point(704, 156 + (i * 21))
                .Size = New Size(51, 20)
                .BackColor = Color.LimeGreen
                .BorderStyle = BorderStyle.FixedSingle
            End With
        Next
        pic14(4).Dispose()
        pic14(5).Dispose()
        pic14(6).Dispose()
        pic14(7).Dispose()
        pic14(8).Dispose()
        pic14(0).BackColor = Color.SeaGreen
        pic14(1).BackColor = Color.White
        pic14(2).BackColor = Color.White
        pic14(9).BackColor = Color.Coral
        pic14(10).BackColor = Color.Coral
    End Sub


End Class