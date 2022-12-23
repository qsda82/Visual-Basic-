Public Class L3P1
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
                vx(tmp) = 7
                vy(tmp) = -7
            End If

            If BHitPoint < 0.33 Then
                vx(tmp) = -7
                vy(tmp) = -7
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
            P.Top = 460 '調整球拍的y座標
            P.Left = (Me.ClientSize.Width - P.Width) / 2 '球拍水平置中
            spacecheck = True
            New_ball()
        ElseIf Dietimes = 2 Then
            Label6.Image = Nothing
            Label5.Image = Nothing
            MsgBox("你死了，還剩1次機會")
            P.Top = 460 '調整球拍的y座標
            P.Left = (Me.ClientSize.Width - P.Width) / 2 '球拍水平置中
            spacecheck = True
            New_ball()
        ElseIf Dietimes = 3 Then
            Label6.Image = Nothing
            Label5.Image = Nothing
            Label4.Image = Nothing
            YOUDIE.Button1.Tag = 7
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
            If check = 85 Then
                Timer1.Stop()
                FINISH.Show()
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
        vy(ball_num) = 7
        ball(ball_num) = New Label()
        With ball(ball_num)
            .Location = New Point(P.Location.X + P.Size.Width / 2 - 15, 430)
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
    Dim balltimes As Integer = 0
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
        If spacecheck = False Then
            If (e.KeyValue = Keys.B) Then
                balltimes = balltimes + 1
                If balltimes <= 5 Then
                    New_ball()
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
        Label6.Image = My.Resources.love
        Label5.Image = My.Resources.love
        Label4.Image = My.Resources.love
        ptimes = 0
        Dietimes = 0
        balltimes = 0
        t = 0
        xx = False
        n = 0
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
        P.Top = 460 '調整球拍的y座標
        P.Left = (Me.ClientSize.Width - P.Width) / 2 '球拍水平置中
        P.Width = 131
        spacecheck = True
        New_ball()
        Dim i As Integer
        ''-----------------------彥---------------------------------------
        Dim pica(33) As PictureBox
        For i = 0 To 32
            If pica(i) Is Nothing Then
                pica(i) = New PictureBox
                Me.Controls.Add(pica(i))
            End If

            With pica(i)
                .Enabled = True
                .Size = New Size(51, 20)
                .BackColor = Color.IndianRed
                .BorderStyle = BorderStyle.FixedSingle
            End With
        Next
        pica(0).Location = New Point(144, 56)
        pica(1).Location = New Point(67, 77)
        pica(2).Location = New Point(119, 77)
        pica(3).Location = New Point(171, 77)
        pica(4).Location = New Point(223, 77)
        pica(5).Location = New Point(92, 98)
        pica(6).Location = New Point(196, 98)
        pica(7).Location = New Point(119, 119)
        pica(8).Location = New Point(171, 119)
        pica(9).Location = New Point(92, 140)
        pica(10).Location = New Point(196, 140)
        pica(11).Location = New Point(42, 182)
        pica(12).Location = New Point(42, 203)
        pica(13).Location = New Point(42, 224)
        pica(14).Location = New Point(28, 245)
        pica(15).Location = New Point(14, 266)
        pica(16).Location = New Point(0, 287)
        pica(17).Location = New Point(98, 208)
        pica(18).Location = New Point(150, 201)
        pica(19).Location = New Point(202, 194)
        pica(20).Location = New Point(82, 256)
        pica(21).Location = New Point(134, 249)
        pica(22).Location = New Point(186, 242)
        pica(23).Location = New Point(238, 235)
        pica(24).Location = New Point(82, 301)
        pica(25).Location = New Point(134, 294)
        pica(26).Location = New Point(186, 287)
        pica(27).Location = New Point(238, 280)
        pica(28).Location = New Point(42, 161)
        pica(29).Location = New Point(94, 161)
        pica(30).Location = New Point(146, 161)
        pica(31).Location = New Point(198, 161)
        pica(32).Location = New Point(250, 161)
        ''------------------賢---------------------------
        Dim picb(52)
        For i = 0 To 51
            If picb(i) Is Nothing Then
                picb(i) = New PictureBox
                Me.Controls.Add(picb(i))
            End If

            With picb(i)
                .Enabled = True
                .Size = New Size(51, 20)
                .BackColor = Color.SteelBlue
                .BorderStyle = BorderStyle.FixedSingle
            End With
        Next
        picb(1).location = New Point(400, 56)
        picb(2).location = New Point(452, 56)
        picb(3).location = New Point(504, 56)
        picb(4).location = New Point(400, 77)
        picb(5).location = New Point(478, 77)
        picb(6).location = New Point(400, 98)
        picb(7).location = New Point(452, 98)
        picb(8).location = New Point(504, 98)
        picb(9).location = New Point(400, 119)
        picb(10).location = New Point(452, 119)
        picb(11).location = New Point(504, 119)
        picb(12).location = New Point(400, 140)
        picb(13).location = New Point(478, 140)
        picb(14).location = New Point(400, 161)
        picb(15).location = New Point(452, 161)
        picb(16).location = New Point(504, 161)
        picb(17).location = New Point(582, 56)
        picb(18).location = New Point(634, 56)
        picb(19).location = New Point(686, 56)
        picb(20).location = New Point(582, 77)
        picb(21).location = New Point(686, 77)
        picb(22).location = New Point(634, 98)
        picb(23).location = New Point(607, 119)
        picb(24).location = New Point(659, 119)
        picb(25).location = New Point(582, 140)
        picb(0).location = New Point(686, 140)
        picb(26).location = New Point(442, 188)
        picb(27).location = New Point(442, 209)
        picb(28).location = New Point(442, 230)
        picb(29).location = New Point(442, 251)
        picb(30).location = New Point(442, 272)
        picb(31).location = New Point(650, 188)
        picb(32).location = New Point(650, 209)
        picb(33).location = New Point(650, 230)
        picb(34).location = New Point(650, 251)
        picb(35).location = New Point(650, 272)
        picb(36).location = New Point(494, 188)
        picb(37).location = New Point(546, 188)
        picb(38).location = New Point(598, 188)
        picb(39).location = New Point(494, 272)
        picb(40).location = New Point(546, 272)
        picb(41).location = New Point(598, 272)
        picb(42).location = New Point(494, 216)
        picb(43).location = New Point(494, 244)
        picb(44).location = New Point(546, 216)
        picb(45).location = New Point(546, 244)
        picb(46).location = New Point(598, 216)
        picb(47).location = New Point(598, 244)
        picb(48).location = New Point(509, 293)
        picb(49).location = New Point(583, 293)
        picb(50).location = New Point(462, 314)
        picb(51).location = New Point(630, 314)
    End Sub


    Dim rand As New Random
    Function AAA()
        Dim LX As Integer
        Dim LY As Integer
        Dim style As Integer

        LX = rand.Next(0, 753)
        LY = rand.Next(337, 428)
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

    Private Sub L3P1_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        New_ball()
        Me.Width = 800
        Me.Height = 520
        Dim i As Integer
        ''-----------------------彥---------------------------------------
        Dim pica(33) As PictureBox
        For i = 0 To 32
            If pica(i) Is Nothing Then
                pica(i) = New PictureBox
                Me.Controls.Add(pica(i))
            End If

            With pica(i)
                .Enabled = True
                .Size = New Size(51, 20)
                .BackColor = Color.IndianRed
                .BorderStyle = BorderStyle.FixedSingle
            End With
        Next
        pica(0).Location = New Point(144, 56)
        pica(1).Location = New Point(67, 77)
        pica(2).Location = New Point(119, 77)
        pica(3).Location = New Point(171, 77)
        pica(4).Location = New Point(223, 77)
        pica(5).Location = New Point(92, 98)
        pica(6).Location = New Point(196, 98)
        pica(7).Location = New Point(119, 119)
        pica(8).Location = New Point(171, 119)
        pica(9).Location = New Point(92, 140)
        pica(10).Location = New Point(196, 140)
        pica(11).Location = New Point(42, 182)
        pica(12).Location = New Point(42, 203)
        pica(13).Location = New Point(42, 224)
        pica(14).Location = New Point(28, 245)
        pica(15).Location = New Point(14, 266)
        pica(16).Location = New Point(0, 287)
        pica(17).Location = New Point(98, 208)
        pica(18).Location = New Point(150, 201)
        pica(19).Location = New Point(202, 194)
        pica(20).Location = New Point(82, 256)
        pica(21).Location = New Point(134, 249)
        pica(22).Location = New Point(186, 242)
        pica(23).Location = New Point(238, 235)
        pica(24).Location = New Point(82, 301)
        pica(25).Location = New Point(134, 294)
        pica(26).Location = New Point(186, 287)
        pica(27).Location = New Point(238, 280)
        pica(28).Location = New Point(42, 161)
        pica(29).Location = New Point(94, 161)
        pica(30).Location = New Point(146, 161)
        pica(31).Location = New Point(198, 161)
        pica(32).Location = New Point(250, 161)
        ''------------------賢---------------------------
        Dim picb(52)
        For i = 0 To 51
            If picb(i) Is Nothing Then
                picb(i) = New PictureBox
                Me.Controls.Add(picb(i))
            End If

            With picb(i)
                .Enabled = True
                .Size = New Size(51, 20)
                .BackColor = Color.SteelBlue
                .BorderStyle = BorderStyle.FixedSingle
            End With
        Next
        picb(1).location = New Point(400, 56)
        picb(2).location = New Point(452, 56)
        picb(3).location = New Point(504, 56)
        picb(4).location = New Point(400, 77)
        picb(5).location = New Point(478, 77)
        picb(6).location = New Point(400, 98)
        picb(7).location = New Point(452, 98)
        picb(8).location = New Point(504, 98)
        picb(9).location = New Point(400, 119)
        picb(10).location = New Point(452, 119)
        picb(11).location = New Point(504, 119)
        picb(12).location = New Point(400, 140)
        picb(13).location = New Point(478, 140)
        picb(14).location = New Point(400, 161)
        picb(15).location = New Point(452, 161)
        picb(16).location = New Point(504, 161)
        picb(17).location = New Point(582, 56)
        picb(18).location = New Point(634, 56)
        picb(19).location = New Point(686, 56)
        picb(20).location = New Point(582, 77)
        picb(21).location = New Point(686, 77)
        picb(22).location = New Point(634, 98)
        picb(23).location = New Point(607, 119)
        picb(24).location = New Point(659, 119)
        picb(25).location = New Point(582, 140)
        picb(0).location = New Point(686, 140)
        picb(26).location = New Point(442, 188)
        picb(27).location = New Point(442, 209)
        picb(28).location = New Point(442, 230)
        picb(29).location = New Point(442, 251)
        picb(30).location = New Point(442, 272)
        picb(31).location = New Point(650, 188)
        picb(32).location = New Point(650, 209)
        picb(33).location = New Point(650, 230)
        picb(34).location = New Point(650, 251)
        picb(35).location = New Point(650, 272)
        picb(36).location = New Point(494, 188)
        picb(37).location = New Point(546, 188)
        picb(38).location = New Point(598, 188)
        picb(39).location = New Point(494, 272)
        picb(40).location = New Point(546, 272)
        picb(41).location = New Point(598, 272)
        picb(42).location = New Point(494, 216)
        picb(43).location = New Point(494, 244)
        picb(44).location = New Point(546, 216)
        picb(45).location = New Point(546, 244)
        picb(46).location = New Point(598, 216)
        picb(47).location = New Point(598, 244)
        picb(48).location = New Point(509, 293)
        picb(49).location = New Point(583, 293)
        picb(50).location = New Point(462, 314)
        picb(51).location = New Point(630, 314)
    End Sub
End Class