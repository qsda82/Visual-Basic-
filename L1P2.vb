Public Class L1P2

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
                vx(tmp) = 3
                vy(tmp) = -3
            End If

            If BHitPoint < 0.33 Then
                vx(tmp) = -3
                vy(tmp) = -3
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
            P.Top = 620 '調整球拍的y座標
            P.Left = (Me.ClientSize.Width - P.Width) / 2 '球拍水平置中
            spacecheck = True
            New_ball()
        ElseIf Dietimes = 2 Then
            Label6.Image = Nothing
            Label5.Image = Nothing
            MsgBox("你死了，還剩1次機會")
            P.Top = 620 '調整球拍的y座標
            P.Left = (Me.ClientSize.Width - P.Width) / 2 '球拍水平置中
            spacecheck = True
            New_ball()
        ElseIf Dietimes = 3 Then
            Label6.Image = Nothing
            Label5.Image = Nothing
            Label4.Image = Nothing
            YOUDIE.Button1.Tag = 2
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
            If check = 200 Then

                Timer1.Stop()
                PASS.Button1.Tag = 2
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
            n = 0
            Timer3.Enabled = True
            Timer4.Enabled = False
            check = check - 1

        ElseIf Q.Tag = 2 Then
            P.Width = P.Width + 20
            check = check - 1

        ElseIf Q.Tag = 3 Then
            n = 0
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
        vy(ball_num) = 3
        ball(ball_num) = New Label()
        With ball(ball_num)
            .Location = New Point(P.Location.X + P.Size.Width / 2 - 15, 590)
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
        n = 0
        t = 0
        xx = False
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
        P.Top = 620 '調整球拍的y座標
        P.Left = (Me.ClientSize.Width - P.Width) / 2 '球拍水平置中
        P.Width = 131
        spacecheck = True
        New_ball()
        Dim i As Integer
        Dim picma(220) As PictureBox
        For i = 0 To 219
            If picma(i) Is Nothing Then
                picma(i) = New PictureBox
                Me.Controls.Add(picma(i))
            End If

            With picma(i)
                .Enabled = True
                .Size = New Size(51, 20)
                .BorderStyle = BorderStyle.FixedSingle
            End With
        Next
        For i = 0 To 46
            picma(i).BackColor = Color.Red
        Next
        picma(0).Location = New Point(222, 56)
        picma(1).Location = New Point(274, 56)
        picma(2).Location = New Point(326, 56)
        picma(3).Location = New Point(378, 56)
        picma(4).Location = New Point(430, 56)
        picma(5).Location = New Point(170, 77)
        picma(6).Location = New Point(222, 77)
        picma(7).Location = New Point(274, 77)
        picma(8).Location = New Point(326, 77)
        picma(9).Location = New Point(378, 77)
        picma(10).Location = New Point(430, 77)
        picma(11).Location = New Point(482, 77)
        picma(12).Location = New Point(534, 77)
        picma(13).Location = New Point(586, 77)
        picma(14).Location = New Point(274, 329)
        picma(15).Location = New Point(274, 350)
        picma(16).Location = New Point(274, 371)
        picma(17).Location = New Point(430, 350)
        picma(18).Location = New Point(430, 371)
        picma(19).Location = New Point(326, 371)
        picma(20).Location = New Point(378, 371)
        picma(21).Location = New Point(222, 392)
        picma(22).Location = New Point(326, 392)
        picma(23).Location = New Point(378, 392)
        picma(24).Location = New Point(482, 392)
        picma(25).Location = New Point(222, 413)
        picma(26).Location = New Point(274, 413)
        picma(27).Location = New Point(326, 413)
        picma(28).Location = New Point(378, 413)
        picma(29).Location = New Point(430, 413)
        picma(30).Location = New Point(482, 413)
        picma(31).Location = New Point(170, 434)
        picma(32).Location = New Point(222, 434)
        picma(33).Location = New Point(274, 434)
        picma(34).Location = New Point(326, 434)
        picma(35).Location = New Point(378, 434)
        picma(36).Location = New Point(430, 434)
        picma(37).Location = New Point(482, 434)
        picma(38).Location = New Point(534, 434)
        picma(39).Location = New Point(222, 455)
        picma(40).Location = New Point(274, 455)
        picma(41).Location = New Point(326, 455)
        picma(42).Location = New Point(378, 455)
        picma(43).Location = New Point(430, 455)
        picma(44).Location = New Point(482, 455)
        picma(45).Location = New Point(534, 455)
        picma(46).Location = New Point(170, 455)

        For i = 47 To 100
            picma(i).BackColor = Color.Chocolate
        Next
        picma(47).Location = New Point(170, 98)
        picma(48).Location = New Point(222, 98)
        picma(49).Location = New Point(274, 98)
        picma(50).Location = New Point(430, 98)
        picma(51).Location = New Point(170, 119)
        picma(52).Location = New Point(222, 119)
        picma(53).Location = New Point(274, 119)
        picma(54).Location = New Point(430, 119)
        picma(55).Location = New Point(118, 140)
        picma(56).Location = New Point(118, 161)
        picma(57).Location = New Point(118, 182)
        picma(58).Location = New Point(118, 203)
        picma(59).Location = New Point(118, 224)
        picma(60).Location = New Point(118, 245)
        picma(61).Location = New Point(118, 266)
        picma(62).Location = New Point(170, 224)
        picma(63).Location = New Point(170, 245)
        picma(64).Location = New Point(170, 266)
        picma(65).Location = New Point(222, 140)
        picma(66).Location = New Point(222, 161)
        picma(67).Location = New Point(222, 182)
        picma(68).Location = New Point(222, 203)
        picma(69).Location = New Point(274, 182)
        picma(70).Location = New Point(274, 203)
        picma(71).Location = New Point(586, 224)
        picma(72).Location = New Point(430, 140)
        picma(73).Location = New Point(430, 161)
        picma(74).Location = New Point(482, 266)
        picma(75).Location = New Point(430, 224)
        picma(76).Location = New Point(430, 245)
        picma(77).Location = New Point(430, 266)
        picma(78).Location = New Point(586, 245)
        picma(79).Location = New Point(482, 182)
        picma(80).Location = New Point(482, 203)
        picma(81).Location = New Point(482, 224)
        picma(82).Location = New Point(482, 245)
        picma(83).Location = New Point(534, 224)
        picma(84).Location = New Point(534, 245)
        picma(85).Location = New Point(534, 266)
        picma(86).Location = New Point(586, 476)
        picma(87).Location = New Point(638, 497)
        picma(88).Location = New Point(586, 266)
        picma(89).Location = New Point(118, 497)
        picma(90).Location = New Point(170, 476)
        picma(91).Location = New Point(170, 497)
        picma(92).Location = New Point(222, 476)
        picma(93).Location = New Point(222, 497)
        picma(94).Location = New Point(274, 476)
        picma(95).Location = New Point(274, 497)
        picma(96).Location = New Point(482, 476)
        picma(97).Location = New Point(482, 497)
        picma(98).Location = New Point(534, 476)
        picma(99).Location = New Point(534, 497)
        picma(100).Location = New Point(586, 497)

        For i = 101 To 176
            picma(i).BackColor = Color.SandyBrown
        Next
        picma(101).Location = New Point(170, 140)
        picma(102).Location = New Point(170, 161)
        picma(103).Location = New Point(170, 182)
        picma(104).Location = New Point(170, 203)
        picma(105).Location = New Point(222, 224)
        picma(106).Location = New Point(222, 245)
        picma(107).Location = New Point(222, 266)
        picma(108).Location = New Point(222, 287)
        picma(109).Location = New Point(222, 308)
        picma(110).Location = New Point(274, 140)
        picma(111).Location = New Point(274, 161)
        picma(112).Location = New Point(274, 224)
        picma(113).Location = New Point(274, 245)
        picma(114).Location = New Point(274, 266)
        picma(115).Location = New Point(274, 287)
        picma(116).Location = New Point(274, 308)
        picma(117).Location = New Point(326, 98)
        picma(118).Location = New Point(326, 119)
        picma(119).Location = New Point(326, 140)
        picma(120).Location = New Point(326, 161)
        picma(121).Location = New Point(326, 182)
        picma(122).Location = New Point(326, 203)
        picma(123).Location = New Point(326, 224)
        picma(124).Location = New Point(326, 245)
        picma(125).Location = New Point(326, 266)
        picma(126).Location = New Point(326, 287)
        picma(127).Location = New Point(326, 308)
        picma(128).Location = New Point(378, 98)
        picma(129).Location = New Point(378, 119)
        picma(130).Location = New Point(378, 140)
        picma(131).Location = New Point(378, 161)
        picma(132).Location = New Point(378, 182)
        picma(133).Location = New Point(378, 203)
        picma(134).Location = New Point(378, 224)
        picma(135).Location = New Point(378, 245)
        picma(136).Location = New Point(378, 266)
        picma(137).Location = New Point(378, 287)
        picma(138).Location = New Point(378, 308)
        picma(139).Location = New Point(430, 182)
        picma(140).Location = New Point(430, 203)
        picma(141).Location = New Point(430, 287)
        picma(142).Location = New Point(430, 308)
        picma(143).Location = New Point(482, 98)
        picma(144).Location = New Point(482, 119)
        picma(145).Location = New Point(482, 140)
        picma(146).Location = New Point(482, 161)
        picma(147).Location = New Point(482, 287)
        picma(148).Location = New Point(482, 308)
        picma(149).Location = New Point(534, 140)
        picma(150).Location = New Point(534, 161)
        picma(151).Location = New Point(534, 182)
        picma(152).Location = New Point(534, 203)
        picma(153).Location = New Point(534, 287)
        picma(154).Location = New Point(534, 308)
        picma(155).Location = New Point(586, 140)
        picma(156).Location = New Point(586, 161)
        picma(157).Location = New Point(586, 182)
        picma(158).Location = New Point(586, 203)
        picma(159).Location = New Point(638, 182)
        picma(160).Location = New Point(638, 203)
        picma(161).Location = New Point(66, 392)
        picma(162).Location = New Point(66, 413)
        picma(163).Location = New Point(66, 434)
        picma(164).Location = New Point(118, 392)
        picma(165).Location = New Point(118, 413)
        picma(166).Location = New Point(118, 434)
        picma(167).Location = New Point(170, 413)
        picma(168).Location = New Point(274, 392)
        picma(169).Location = New Point(430, 392)
        picma(170).Location = New Point(534, 413)
        picma(171).Location = New Point(586, 392)
        picma(172).Location = New Point(586, 413)
        picma(173).Location = New Point(586, 434)
        picma(174).Location = New Point(638, 392)
        picma(175).Location = New Point(638, 413)
        picma(176).Location = New Point(638, 434)

        For i = 177 To 199
            picma(i).BackColor = Color.SteelBlue
        Next
        picma(177).Location = New Point(170, 329)
        picma(178).Location = New Point(222, 329)
        picma(179).Location = New Point(326, 329)
        picma(180).Location = New Point(378, 329)
        picma(181).Location = New Point(430, 329)
        picma(182).Location = New Point(118, 350)
        picma(183).Location = New Point(170, 350)
        picma(184).Location = New Point(222, 350)
        picma(185).Location = New Point(326, 350)
        picma(186).Location = New Point(378, 350)
        picma(187).Location = New Point(482, 350)
        picma(188).Location = New Point(534, 350)
        picma(189).Location = New Point(586, 350)
        picma(190).Location = New Point(66, 371)
        picma(191).Location = New Point(118, 371)
        picma(192).Location = New Point(170, 371)
        picma(193).Location = New Point(222, 371)
        picma(194).Location = New Point(482, 371)
        picma(195).Location = New Point(534, 371)
        picma(196).Location = New Point(586, 371)
        picma(197).Location = New Point(638, 371)
        picma(198).Location = New Point(170, 392)
        picma(199).Location = New Point(534, 392)


    End Sub


    Dim rand As New Random
    Function AAA()
        Dim LX As Integer
        Dim LY As Integer
        Dim style As Integer

        LX = rand.Next(0, 750)
        LY = rand.Next(535, 590)
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

    Private Sub L1P1_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        New_ball()
        Me.Width = 800
        Me.Height = 680
        Dim i As Integer

        Dim picma(220) As PictureBox
        For i = 0 To 219
            If picma(i) Is Nothing Then
                picma(i) = New PictureBox
                Me.Controls.Add(picma(i))
            End If

            With picma(i)
                .Enabled = True
                .Size = New Size(51, 20)
                .BorderStyle = BorderStyle.FixedSingle
            End With
        Next
        For i = 0 To 46
            picma(i).BackColor = Color.Red
        Next
        picma(0).Location = New Point(222, 56)
        picma(1).Location = New Point(274, 56)
        picma(2).Location = New Point(326, 56)
        picma(3).Location = New Point(378, 56)
        picma(4).Location = New Point(430, 56)
        picma(5).Location = New Point(170, 77)
        picma(6).Location = New Point(222, 77)
        picma(7).Location = New Point(274, 77)
        picma(8).Location = New Point(326, 77)
        picma(9).Location = New Point(378, 77)
        picma(10).Location = New Point(430, 77)
        picma(11).Location = New Point(482, 77)
        picma(12).Location = New Point(534, 77)
        picma(13).Location = New Point(586, 77)
        picma(14).Location = New Point(274, 329)
        picma(15).Location = New Point(274, 350)
        picma(16).Location = New Point(274, 371)
        picma(17).Location = New Point(430, 350)
        picma(18).Location = New Point(430, 371)
        picma(19).Location = New Point(326, 371)
        picma(20).Location = New Point(378, 371)
        picma(21).Location = New Point(222, 392)
        picma(22).Location = New Point(326, 392)
        picma(23).Location = New Point(378, 392)
        picma(24).Location = New Point(482, 392)
        picma(25).Location = New Point(222, 413)
        picma(26).Location = New Point(274, 413)
        picma(27).Location = New Point(326, 413)
        picma(28).Location = New Point(378, 413)
        picma(29).Location = New Point(430, 413)
        picma(30).Location = New Point(482, 413)
        picma(31).Location = New Point(170, 434)
        picma(32).Location = New Point(222, 434)
        picma(33).Location = New Point(274, 434)
        picma(34).Location = New Point(326, 434)
        picma(35).Location = New Point(378, 434)
        picma(36).Location = New Point(430, 434)
        picma(37).Location = New Point(482, 434)
        picma(38).Location = New Point(534, 434)
        picma(39).Location = New Point(222, 455)
        picma(40).Location = New Point(274, 455)
        picma(41).Location = New Point(326, 455)
        picma(42).Location = New Point(378, 455)
        picma(43).Location = New Point(430, 455)
        picma(44).Location = New Point(482, 455)
        picma(45).Location = New Point(534, 455)
        picma(46).Location = New Point(170, 455)

        For i = 47 To 100
            picma(i).BackColor = Color.Chocolate
        Next
        picma(47).Location = New Point(170, 98)
        picma(48).Location = New Point(222, 98)
        picma(49).Location = New Point(274, 98)
        picma(50).Location = New Point(430, 98)
        picma(51).Location = New Point(170, 119)
        picma(52).Location = New Point(222, 119)
        picma(53).Location = New Point(274, 119)
        picma(54).Location = New Point(430, 119)
        picma(55).Location = New Point(118, 140)
        picma(56).Location = New Point(118, 161)
        picma(57).Location = New Point(118, 182)
        picma(58).Location = New Point(118, 203)
        picma(59).Location = New Point(118, 224)
        picma(60).Location = New Point(118, 245)
        picma(61).Location = New Point(118, 266)
        picma(62).Location = New Point(170, 224)
        picma(63).Location = New Point(170, 245)
        picma(64).Location = New Point(170, 266)
        picma(65).Location = New Point(222, 140)
        picma(66).Location = New Point(222, 161)
        picma(67).Location = New Point(222, 182)
        picma(68).Location = New Point(222, 203)
        picma(69).Location = New Point(274, 182)
        picma(70).Location = New Point(274, 203)
        picma(71).Location = New Point(586, 224)
        picma(72).Location = New Point(430, 140)
        picma(73).Location = New Point(430, 161)
        picma(74).Location = New Point(482, 266)
        picma(75).Location = New Point(430, 224)
        picma(76).Location = New Point(430, 245)
        picma(77).Location = New Point(430, 266)
        picma(78).Location = New Point(586, 245)
        picma(79).Location = New Point(482, 182)
        picma(80).Location = New Point(482, 203)
        picma(81).Location = New Point(482, 224)
        picma(82).Location = New Point(482, 245)
        picma(83).Location = New Point(534, 224)
        picma(84).Location = New Point(534, 245)
        picma(85).Location = New Point(534, 266)
        picma(86).Location = New Point(586, 476)
        picma(87).Location = New Point(638, 497)
        picma(88).Location = New Point(586, 266)
        picma(89).Location = New Point(118, 497)
        picma(90).Location = New Point(170, 476)
        picma(91).Location = New Point(170, 497)
        picma(92).Location = New Point(222, 476)
        picma(93).Location = New Point(222, 497)
        picma(94).Location = New Point(274, 476)
        picma(95).Location = New Point(274, 497)
        picma(96).Location = New Point(482, 476)
        picma(97).Location = New Point(482, 497)
        picma(98).Location = New Point(534, 476)
        picma(99).Location = New Point(534, 497)
        picma(100).Location = New Point(586, 497)

        For i = 101 To 176
            picma(i).BackColor = Color.SandyBrown
        Next
        picma(101).Location = New Point(170, 140)
        picma(102).Location = New Point(170, 161)
        picma(103).Location = New Point(170, 182)
        picma(104).Location = New Point(170, 203)
        picma(105).Location = New Point(222, 224)
        picma(106).Location = New Point(222, 245)
        picma(107).Location = New Point(222, 266)
        picma(108).Location = New Point(222, 287)
        picma(109).Location = New Point(222, 308)
        picma(110).Location = New Point(274, 140)
        picma(111).Location = New Point(274, 161)
        picma(112).Location = New Point(274, 224)
        picma(113).Location = New Point(274, 245)
        picma(114).Location = New Point(274, 266)
        picma(115).Location = New Point(274, 287)
        picma(116).Location = New Point(274, 308)
        picma(117).Location = New Point(326, 98)
        picma(118).Location = New Point(326, 119)
        picma(119).Location = New Point(326, 140)
        picma(120).Location = New Point(326, 161)
        picma(121).Location = New Point(326, 182)
        picma(122).Location = New Point(326, 203)
        picma(123).Location = New Point(326, 224)
        picma(124).Location = New Point(326, 245)
        picma(125).Location = New Point(326, 266)
        picma(126).Location = New Point(326, 287)
        picma(127).Location = New Point(326, 308)
        picma(128).Location = New Point(378, 98)
        picma(129).Location = New Point(378, 119)
        picma(130).Location = New Point(378, 140)
        picma(131).Location = New Point(378, 161)
        picma(132).Location = New Point(378, 182)
        picma(133).Location = New Point(378, 203)
        picma(134).Location = New Point(378, 224)
        picma(135).Location = New Point(378, 245)
        picma(136).Location = New Point(378, 266)
        picma(137).Location = New Point(378, 287)
        picma(138).Location = New Point(378, 308)
        picma(139).Location = New Point(430, 182)
        picma(140).Location = New Point(430, 203)
        picma(141).Location = New Point(430, 287)
        picma(142).Location = New Point(430, 308)
        picma(143).Location = New Point(482, 98)
        picma(144).Location = New Point(482, 119)
        picma(145).Location = New Point(482, 140)
        picma(146).Location = New Point(482, 161)
        picma(147).Location = New Point(482, 287)
        picma(148).Location = New Point(482, 308)
        picma(149).Location = New Point(534, 140)
        picma(150).Location = New Point(534, 161)
        picma(151).Location = New Point(534, 182)
        picma(152).Location = New Point(534, 203)
        picma(153).Location = New Point(534, 287)
        picma(154).Location = New Point(534, 308)
        picma(155).Location = New Point(586, 140)
        picma(156).Location = New Point(586, 161)
        picma(157).Location = New Point(586, 182)
        picma(158).Location = New Point(586, 203)
        picma(159).Location = New Point(638, 182)
        picma(160).Location = New Point(638, 203)
        picma(161).Location = New Point(66, 392)
        picma(162).Location = New Point(66, 413)
        picma(163).Location = New Point(66, 434)
        picma(164).Location = New Point(118, 392)
        picma(165).Location = New Point(118, 413)
        picma(166).Location = New Point(118, 434)
        picma(167).Location = New Point(170, 413)
        picma(168).Location = New Point(274, 392)
        picma(169).Location = New Point(430, 392)
        picma(170).Location = New Point(534, 413)
        picma(171).Location = New Point(586, 392)
        picma(172).Location = New Point(586, 413)
        picma(173).Location = New Point(586, 434)
        picma(174).Location = New Point(638, 392)
        picma(175).Location = New Point(638, 413)
        picma(176).Location = New Point(638, 434)

        For i = 177 To 199
            picma(i).BackColor = Color.SteelBlue
        Next
        picma(177).Location = New Point(170, 329)
        picma(178).Location = New Point(222, 329)
        picma(179).Location = New Point(326, 329)
        picma(180).Location = New Point(378, 329)
        picma(181).Location = New Point(430, 329)
        picma(182).Location = New Point(118, 350)
        picma(183).Location = New Point(170, 350)
        picma(184).Location = New Point(222, 350)
        picma(185).Location = New Point(326, 350)
        picma(186).Location = New Point(378, 350)
        picma(187).Location = New Point(482, 350)
        picma(188).Location = New Point(534, 350)
        picma(189).Location = New Point(586, 350)
        picma(190).Location = New Point(66, 371)
        picma(191).Location = New Point(118, 371)
        picma(192).Location = New Point(170, 371)
        picma(193).Location = New Point(222, 371)
        picma(194).Location = New Point(482, 371)
        picma(195).Location = New Point(534, 371)
        picma(196).Location = New Point(586, 371)
        picma(197).Location = New Point(638, 371)
        picma(198).Location = New Point(170, 392)
        picma(199).Location = New Point(534, 392)
    End Sub


End Class