Public Class L1P1
    Dim check As Integer = 0
    Dim spacecheck As Boolean = True
    Dim xx As Boolean = True
    '按下空白建開始
    Private Sub Form2_KeyDown(sender As Object, e As System.Windows.Forms.KeyEventArgs) Handles Me.KeyDown
        If (e.KeyValue = Keys.Space) Then
            Timer1.Start() '啟動球的運動
            Timer2.Enabled = True '道具計時開始
            spacecheck = False '開始前讓球黏著板子
        End If
        '當暫停時，按空白鍵不會讓球開始動
        If ptimes = 1 Then
            Timer1.Enabled = False
        End If
    End Sub


    Dim Dietimes As Integer = 0
    '球的各種運動和輸贏判斷
    Private Sub Timer1_Tick(sender As Object, e As EventArgs) Handles Timer1.Tick
        For i As Integer = 1 To ball_num
            If_dead()
            Ball_move(ball(i))
            If_Win(ball(i))
        Next

    End Sub
    '球的運動
    Function Ball_move(B As Label) As Boolean
        Dim tmp As Integer = B.Tag '取得是第幾顆球
        If tmp = 0 Then
            Return False
        End If

        B.Left = B.Left + vx(tmp) 'X方向移動
        B.Top = B.Top + vy(tmp)  'Y方向移動

        If B.Left < 0 Then vx(tmp) = Math.Abs(vx(tmp)) '碰左牆，往右

        If B.Right > Me.ClientSize.Width Then vx(tmp) = -Math.Abs(vx(tmp)) '碰右牆，往左

        If B.Top < 60 Then '碰頂端
            vy(tmp) = Math.Abs(vy(tmp)) '往下
        End If

        Dim BCenter As Single = (B.Left + B.Right) / 2 '球中心點X座標

        '碰到桿子
        If B.Bottom > P.Top And BCenter > P.Left And BCenter < P.Right Then
            vy(tmp) = -Math.Abs(vy(tmp)) '像上彈
            Dim BHitPoint As Single = (BCenter - P.Left) / P.Width '計算擊球點

            '將桿子切成三等份，若碰到中間部分，球以入射角=反射角方式往上彈
            '若碰到桿子右邊部分， 像右上方彈
            If BHitPoint > 0.66 Then
                vx(tmp) = 3
                vy(tmp) = -3
            End If
            '若碰到桿子左邊部分，像左上方彈
            If BHitPoint < 0.33 Then
                vx(tmp) = -3
                vy(tmp) = -3
            End If
            '碰到桿子右邊的寬
        ElseIf (B.Bottom > P.Top And BCenter > P.Left) Or (B.Bottom > P.Top And BCenter < P.Right) Then
            vy(tmp) = Math.Abs(vy(tmp)) '往右下彈
            '碰到桿子左邊的寬
        ElseIf (B.Bottom > P.Bottom And BCenter > P.Left) Or (B.Bottom > P.Bottom And BCenter < P.Right) Then
            vy(tmp) = Math.Abs(vy(tmp)) '往左下彈

        End If
        Return True
    End Function
    '遊戲若輸了
    Function If_dead() As Boolean
        Dim dead As Boolean = True
        '球掉落到桿子下方，並碰到下方邊界
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

        Dietimes = Dietimes + 1 '死亡次數
        '若死亡時，道具和球功能都會停止
        Timer1.Stop()
        Timer2.Enabled = False
        Timer3.Enabled = False
        Timer4.Enabled = False
        '將道具閃頻和黑頻遮蓋的物件顯示出來
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
        '將死前還沒碰到的道具刪除
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
        '死亡次數的判斷
        If Dietimes = 1 Then
            Label6.Image = Nothing
            MsgBox("你死了，還剩2次機會")
            P.Top = 517 '調整球拍的y座標
            P.Left = (Me.ClientSize.Width - P.Width) / 2 '球拍水平置中
            spacecheck = True
            New_ball()
        ElseIf Dietimes = 2 Then
            Label6.Image = Nothing
            Label5.Image = Nothing
            MsgBox("你死了，還剩1次機會")
            P.Top = 517 '調整球拍的y座標
            P.Left = (Me.ClientSize.Width - P.Width) / 2 '球拍水平置中
            spacecheck = True
            New_ball()
        ElseIf Dietimes = 3 Then
            Label6.Image = Nothing
            Label5.Image = Nothing
            Label4.Image = Nothing
            YOUDIE.Button1.Tag = 1
            YOUDIE.Show()
            Me.Close()
        End If

        Return True
    End Function
    '遊戲若贏了
    Function If_Win(B As Label) As Boolean
        If xx = False Then
            check = 0
            xx = True
        End If
        '打到磚塊
        For Each q In Me.Controls
            If TypeOf (q) Is PictureBox AndAlso chkHit(q, B) Then
                check = check + 1
            End If
            '磚塊數共123個，打完之後會跳出贏了的FORM
            If check = 123 Then
                Timer1.Stop()
                PASS.Button1.Tag = 1
                PASS.Show()
                Me.Close()
            End If
        Next

        Return True
    End Function

    '球碰到磚塊或道具的反應
    Function chkHit(Q As PictureBox, Bal As Label) As Boolean
        Dim tmp As Integer = Bal.Tag
        If tmp = 0 Then
            Return False
        End If
        If Bal.Right < Q.Left Then Return False '偏左未碰到
        If Bal.Left > Q.Right Then Return False '偏右未碰到 
        If Bal.Top > Q.Bottom Then Return False '偏下未碰到
        If Bal.Bottom < Q.Top Then Return False '偏上未碰到 
        '碰撞目標左側，左轉彎                      
        If Bal.Right >= Q.Left And (Bal.Right - Q.Left) <= Math.Abs(vx(tmp)) Then vx(tmp) = -Math.Abs(vx(tmp))
        '碰撞目標右側，右轉彎                            
        If Bal.Left <= Q.Right And (Q.Right - Bal.Left) <= Math.Abs(vx(tmp)) Then vx(tmp) = Math.Abs(vx(tmp))
        '碰撞目標底部，往下彈
        If Bal.Top <= Q.Bottom And (Q.Bottom - Bal.Top) <= Math.Abs(vy(tmp)) Then vy(tmp) = Math.Abs(vy(tmp))
        '碰撞目標頂部，往上彈
        If Bal.Bottom >= Q.Top And (Bal.Bottom - Q.Top) <= Math.Abs(vy(tmp)) Then vy(tmp) = -Math.Abs(vy(tmp))
        '碰到道具一
        If Q.Tag = 1 Then

            Timer3.Enabled = True
            Timer4.Enabled = False
            check = check - 1
            '碰到道具二
        ElseIf Q.Tag = 2 Then
            P.Width = P.Width + 20
            check = check - 1
            '碰到道具三
        ElseIf Q.Tag = 3 Then

            Timer4.Enabled = True
            Timer3.Enabled = False
            check = check - 1
            '碰到道具四
        ElseIf Q.Tag = 4 Then
            check = check - 1
            New_ball()
        End If
        Q.Dispose() '刪除磚塊
        Return True '回傳有碰撞
    End Function

    Dim ball() As Label '設立動態陣列
    Dim ball_num As Integer = 0
    Dim vx() As Single  '設立動態陣列
    Dim vy() As Single  '設立動態陣列
    '產生新的球
    Function New_ball() As Integer
        Dim x As Integer = P.Location.X
        ball_num += 1 '每建立一顆新球就增加一次
        ReDim Preserve ball(ball_num) '因為要多一顆球，故重新建立陣列大小
        ReDim Preserve vx(ball_num)   '因為要多一顆球，故重新建立陣列大小，制定新球的X速度
        vx(ball_num) = 0 '球初始X方向
        ReDim Preserve vy(ball_num)   '因為要多一顆球，故重新建立陣列大小，制定新球的Y速度
        vy(ball_num) = 3 '球初始Y方向
        ball(ball_num) = New Label()
        '動態產生球
        With ball(ball_num)
            .Location = New Point(P.Location.X + P.Size.Width / 2 - 15, 485)
            .Size = New Size(28, 28)
            .Image = My.Resources._6663
            .Visible = True
            .Tag = ball_num '方便紀錄是第幾顆球，也就代表是動態陣列中的第幾個陣列
        End With
        '將球加入控制項
        Me.Controls.Add(ball(ball_num))
        ball(ball_num).BringToFront() '球設定的位置可能有其他物件，故將球移至最上層，才不會被其他物件遮蔽
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
            '讓球跟著桿子動
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


    '說明
    Private Sub Label2_Click(sender As Object, e As EventArgs) Handles Label2.Click
        EXPLAN.Show()
    End Sub

    Function DELETE(bal As Label)
        bal.Top = 1000
    End Function
    '重新，將所有設定規回到原本樣子
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
        P.Top = 517 '調整球拍的y座標
        P.Left = (Me.ClientSize.Width - P.Width) / 2 '球拍水平置中
        P.Width = 131
        spacecheck = True
        New_ball()
        Dim pic1(7) As PictureBox
        Dim i As Integer
        For i = 0 To 6
            If pic1(i) Is Nothing Then
                pic1(i) = New PictureBox
                Me.Controls.Add(pic1(i))
            End If

            With pic1(i)
                .Enabled = True
                .Location = New Point(226 + (i * 52), 56)
                .Size = New Size(51, 20)
                .BackColor = Color.IndianRed
                .BorderStyle = BorderStyle.FixedSingle
            End With
        Next

        Dim pic2(8) As PictureBox
        For i = 0 To 7
            If pic2(i) Is Nothing Then
                pic2(i) = New PictureBox
                Me.Controls.Add(pic2(i))
            End If

            With pic2(i)
                .Enabled = True
                .Location = New Point(199 + (i * 52), 77)
                .Size = New Size(51, 20)
                .BackColor = Color.IndianRed
                .BorderStyle = BorderStyle.FixedSingle
            End With
        Next

        Dim pic3(9) As PictureBox
        For i = 0 To 8
            If pic3(i) Is Nothing Then
                pic3(i) = New PictureBox
                Me.Controls.Add(pic3(i))
            End If

            With pic3(i)
                .Enabled = True
                .Location = New Point(174 + (i * 52), 98)
                .Size = New Size(51, 20)
                .BackColor = Color.IndianRed
                .BorderStyle = BorderStyle.FixedSingle
            End With
        Next
        For i = 2 To 6
            pic3(i).BackColor = Color.Khaki
        Next

        Dim pic4(10) As PictureBox
        For i = 0 To 9
            If pic4(i) Is Nothing Then
                pic4(i) = New PictureBox
                Me.Controls.Add(pic4(i))
            End If

            With pic4(i)
                .Enabled = True
                .Location = New Point(142 + (i * 52), 119)
                .Size = New Size(51, 20)
                .BackColor = Color.IndianRed
                .BorderStyle = BorderStyle.FixedSingle
            End With
        Next
        For i = 2 To 7
            pic4(i).BackColor = Color.Khaki
        Next

        Dim pic5(11) As PictureBox
        For i = 0 To 10
            If pic5(i) Is Nothing Then
                pic5(i) = New PictureBox
                Me.Controls.Add(pic5(i))
            End If

            With pic5(i)
                .Enabled = True
                .Location = New Point(107 + (i * 52), 140)
                .Size = New Size(51, 20)
                .BackColor = Color.IndianRed
                .BorderStyle = BorderStyle.FixedSingle
            End With
        Next
        For i = 2 To 8
            pic5(i).BackColor = Color.Khaki
        Next
        For i = 4 To 6
            pic5(i).BackColor = Color.LightSkyBlue
        Next

        Dim pic6(12) As PictureBox
        For i = 0 To 11
            If pic6(i) Is Nothing Then
                pic6(i) = New PictureBox
                Me.Controls.Add(pic6(i))
            End If

            With pic6(i)
                .Enabled = True
                .Location = New Point(77 + (i * 52), 161)
                .Size = New Size(51, 20)
                .BackColor = Color.IndianRed
                .BorderStyle = BorderStyle.FixedSingle
            End With
        Next
        For i = 2 To 9
            pic6(i).BackColor = Color.Khaki
        Next
        For i = 4 To 7
            pic6(i).BackColor = Color.LightSkyBlue
        Next

        Dim pic7(13) As PictureBox
        For i = 0 To 12
            If pic7(i) Is Nothing Then
                pic7(i) = New PictureBox
                Me.Controls.Add(pic7(i))
            End If

            With pic7(i)
                .Enabled = True
                .Location = New Point(50 + (i * 52), 182)
                .Size = New Size(51, 20)
                .BackColor = Color.IndianRed
                .BorderStyle = BorderStyle.FixedSingle
            End With
        Next
        pic7(2).BackColor = Color.Khaki
        pic7(3).BackColor = Color.Khaki
        pic7(4).BackColor = Color.LightSkyBlue
        pic7(5).BackColor = Color.LightSkyBlue
        pic7(7).BackColor = Color.LightSkyBlue
        pic7(8).BackColor = Color.LightSkyBlue
        pic7(9).BackColor = Color.Khaki
        pic7(10).BackColor = Color.Khaki
        pic7(6).Dispose()

        Dim pic8(14) As PictureBox
        For i = 0 To 13
            If pic8(i) Is Nothing Then
                pic8(i) = New PictureBox
                Me.Controls.Add(pic8(i))
            End If

            With pic8(i)
                .Enabled = True
                .Location = New Point(22 + (i * 52), 203)
                .Size = New Size(51, 20)
                .BackColor = Color.IndianRed
                .BorderStyle = BorderStyle.FixedSingle
            End With
        Next
        pic8(2).BackColor = Color.Khaki
        pic8(3).BackColor = Color.Khaki
        pic8(4).BackColor = Color.LightSkyBlue
        pic8(5).BackColor = Color.LightSkyBlue
        pic8(8).BackColor = Color.LightSkyBlue
        pic8(9).BackColor = Color.LightSkyBlue
        pic8(10).BackColor = Color.Khaki
        pic8(11).BackColor = Color.Khaki
        pic8(6).Dispose()
        pic8(7).Dispose()

        Dim pic9(14) As PictureBox
        For i = 0 To 13
            If pic9(i) Is Nothing Then
                pic9(i) = New PictureBox
                Me.Controls.Add(pic9(i))
            End If

            With pic9(i)
                .Enabled = True
                .Location = New Point(3 + (i * 52), 224)
                .Size = New Size(51, 20)
                .BackColor = Color.Gainsboro
                .BorderStyle = BorderStyle.FixedSingle
            End With
        Next
        pic9(0).BackColor = Color.IndianRed
        pic9(1).BackColor = Color.IndianRed
        pic9(3).BackColor = Color.Khaki
        pic9(5).BackColor = Color.LightSkyBlue
        pic9(9).BackColor = Color.LightSkyBlue
        pic9(11).BackColor = Color.Khaki
        pic9(13).BackColor = Color.IndianRed
        pic9(6).Dispose()
        pic9(7).Dispose()
        pic9(8).Dispose()

        Dim pic10(14) As PictureBox
        For i = 0 To 13
            If pic10(i) Is Nothing Then
                pic10(i) = New PictureBox
                Me.Controls.Add(pic10(i))
            End If

            With pic10(i)
                .Enabled = True
                .Location = New Point(22 + (i * 52), 245)
                .Size = New Size(51, 20)
                .BackColor = Color.Gainsboro
                .BorderStyle = BorderStyle.FixedSingle
            End With
        Next
        pic10(6).Dispose()
        pic10(7).Dispose()

        Dim pic11(14) As PictureBox
        For i = 0 To 13
            If pic11(i) Is Nothing Then
                pic11(i) = New PictureBox
                Me.Controls.Add(pic11(i))
            End If

            With pic11(i)
                .Enabled = True
                .Location = New Point(3 + (i * 52), 266)
                .Size = New Size(51, 20)
                .BackColor = Color.Gainsboro
                .BorderStyle = BorderStyle.FixedSingle
            End With
        Next
        pic11(6).Dispose()
        pic11(7).Dispose()

        Dim pic12(7) As PictureBox
        For i = 0 To 6
            If pic12(i) Is Nothing Then
                pic12(i) = New PictureBox
                Me.Controls.Add(pic12(i))
            End If

            With pic12(i)
                .Enabled = True
                .Size = New Size(51, 20)
                .BackColor = Color.Gainsboro
                .BorderStyle = BorderStyle.FixedSingle
            End With
        Next
        pic12(0).Location = New Point(82, 287)
        pic12(1).Location = New Point(134, 287)
        pic12(2).Location = New Point(236, 287)
        pic12(3).Location = New Point(441, 287)
        pic12(4).Location = New Point(493, 287)
        pic12(5).Location = New Point(545, 287)
        pic12(6).Location = New Point(649, 287)

    End Sub


    Dim rand As New Random
    '道具功能
    Function AAA()
        Dim LX As Integer
        Dim LY As Integer
        Dim style As Integer
        '隨機座標和道具
        LX = rand.Next(0, 750)
        LY = rand.Next(333, 487)
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
    '動態產生磚塊
    Private Sub L1P1_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        New_ball()
        Me.Width = 800
        Me.Height = 600
        Dim pic1(7) As PictureBox
        Dim i As Integer
        For i = 0 To 6
            If pic1(i) Is Nothing Then
                pic1(i) = New PictureBox
                Me.Controls.Add(pic1(i))
            End If

            With pic1(i)
                .Enabled = True
                .Location = New Point(226 + (i * 52), 56)
                .Size = New Size(51, 20)
                .BackColor = Color.IndianRed
                .BorderStyle = BorderStyle.FixedSingle
            End With
        Next

        Dim pic2(8) As PictureBox
        For i = 0 To 7
            If pic2(i) Is Nothing Then
                pic2(i) = New PictureBox
                Me.Controls.Add(pic2(i))
            End If

            With pic2(i)
                .Enabled = True
                .Location = New Point(199 + (i * 52), 77)
                .Size = New Size(51, 20)
                .BackColor = Color.IndianRed
                .BorderStyle = BorderStyle.FixedSingle
            End With
        Next

        Dim pic3(9) As PictureBox
        For i = 0 To 8
            If pic3(i) Is Nothing Then
                pic3(i) = New PictureBox
                Me.Controls.Add(pic3(i))
            End If

            With pic3(i)
                .Enabled = True
                .Location = New Point(174 + (i * 52), 98)
                .Size = New Size(51, 20)
                .BackColor = Color.IndianRed
                .BorderStyle = BorderStyle.FixedSingle
            End With
        Next
        For i = 2 To 6
            pic3(i).BackColor = Color.Khaki
        Next

        Dim pic4(10) As PictureBox
        For i = 0 To 9
            If pic4(i) Is Nothing Then
                pic4(i) = New PictureBox
                Me.Controls.Add(pic4(i))
            End If

            With pic4(i)
                .Enabled = True
                .Location = New Point(142 + (i * 52), 119)
                .Size = New Size(51, 20)
                .BackColor = Color.IndianRed
                .BorderStyle = BorderStyle.FixedSingle
            End With
        Next
        For i = 2 To 7
            pic4(i).BackColor = Color.Khaki
        Next

        Dim pic5(11) As PictureBox
        For i = 0 To 10
            If pic5(i) Is Nothing Then
                pic5(i) = New PictureBox
                Me.Controls.Add(pic5(i))
            End If

            With pic5(i)
                .Enabled = True
                .Location = New Point(107 + (i * 52), 140)
                .Size = New Size(51, 20)
                .BackColor = Color.IndianRed
                .BorderStyle = BorderStyle.FixedSingle
            End With
        Next
        For i = 2 To 8
            pic5(i).BackColor = Color.Khaki
        Next
        For i = 4 To 6
            pic5(i).BackColor = Color.LightSkyBlue
        Next

        Dim pic6(12) As PictureBox
        For i = 0 To 11
            If pic6(i) Is Nothing Then
                pic6(i) = New PictureBox
                Me.Controls.Add(pic6(i))
            End If

            With pic6(i)
                .Enabled = True
                .Location = New Point(77 + (i * 52), 161)
                .Size = New Size(51, 20)
                .BackColor = Color.IndianRed
                .BorderStyle = BorderStyle.FixedSingle
            End With
        Next
        For i = 2 To 9
            pic6(i).BackColor = Color.Khaki
        Next
        For i = 4 To 7
            pic6(i).BackColor = Color.LightSkyBlue
        Next

        Dim pic7(13) As PictureBox
        For i = 0 To 12
            If pic7(i) Is Nothing Then
                pic7(i) = New PictureBox
                Me.Controls.Add(pic7(i))
            End If

            With pic7(i)
                .Enabled = True
                .Location = New Point(50 + (i * 52), 182)
                .Size = New Size(51, 20)
                .BackColor = Color.IndianRed
                .BorderStyle = BorderStyle.FixedSingle
            End With
        Next
        pic7(2).BackColor = Color.Khaki
        pic7(3).BackColor = Color.Khaki
        pic7(4).BackColor = Color.LightSkyBlue
        pic7(5).BackColor = Color.LightSkyBlue
        pic7(7).BackColor = Color.LightSkyBlue
        pic7(8).BackColor = Color.LightSkyBlue
        pic7(9).BackColor = Color.Khaki
        pic7(10).BackColor = Color.Khaki
        pic7(6).Dispose()

        Dim pic8(14) As PictureBox
        For i = 0 To 13
            If pic8(i) Is Nothing Then
                pic8(i) = New PictureBox
                Me.Controls.Add(pic8(i))
            End If

            With pic8(i)
                .Enabled = True
                .Location = New Point(22 + (i * 52), 203)
                .Size = New Size(51, 20)
                .BackColor = Color.IndianRed
                .BorderStyle = BorderStyle.FixedSingle
            End With
        Next
        pic8(2).BackColor = Color.Khaki
        pic8(3).BackColor = Color.Khaki
        pic8(4).BackColor = Color.LightSkyBlue
        pic8(5).BackColor = Color.LightSkyBlue
        pic8(8).BackColor = Color.LightSkyBlue
        pic8(9).BackColor = Color.LightSkyBlue
        pic8(10).BackColor = Color.Khaki
        pic8(11).BackColor = Color.Khaki
        pic8(6).Dispose()
        pic8(7).Dispose()

        Dim pic9(14) As PictureBox
        For i = 0 To 13
            If pic9(i) Is Nothing Then
                pic9(i) = New PictureBox
                Me.Controls.Add(pic9(i))
            End If

            With pic9(i)
                .Enabled = True
                .Location = New Point(3 + (i * 52), 224)
                .Size = New Size(51, 20)
                .BackColor = Color.Gainsboro
                .BorderStyle = BorderStyle.FixedSingle
            End With
        Next
        pic9(0).BackColor = Color.IndianRed
        pic9(1).BackColor = Color.IndianRed
        pic9(3).BackColor = Color.Khaki
        pic9(5).BackColor = Color.LightSkyBlue
        pic9(9).BackColor = Color.LightSkyBlue
        pic9(11).BackColor = Color.Khaki
        pic9(13).BackColor = Color.IndianRed
        pic9(6).Dispose()
        pic9(7).Dispose()
        pic9(8).Dispose()

        Dim pic10(14) As PictureBox
        For i = 0 To 13
            If pic10(i) Is Nothing Then
                pic10(i) = New PictureBox
                Me.Controls.Add(pic10(i))
            End If

            With pic10(i)
                .Enabled = True
                .Location = New Point(22 + (i * 52), 245)
                .Size = New Size(51, 20)
                .BackColor = Color.Gainsboro
                .BorderStyle = BorderStyle.FixedSingle
            End With
        Next
        pic10(6).Dispose()
        pic10(7).Dispose()

        Dim pic11(14) As PictureBox
        For i = 0 To 13
            If pic11(i) Is Nothing Then
                pic11(i) = New PictureBox
                Me.Controls.Add(pic11(i))
            End If

            With pic11(i)
                .Enabled = True
                .Location = New Point(3 + (i * 52), 266)
                .Size = New Size(51, 20)
                .BackColor = Color.Gainsboro
                .BorderStyle = BorderStyle.FixedSingle
            End With
        Next
        pic11(6).Dispose()
        pic11(7).Dispose()

        Dim pic12(7) As PictureBox
        For i = 0 To 6
            If pic12(i) Is Nothing Then
                pic12(i) = New PictureBox
                Me.Controls.Add(pic12(i))
            End If

            With pic12(i)
                .Enabled = True
                .Size = New Size(51, 20)
                .BackColor = Color.Gainsboro
                .BorderStyle = BorderStyle.FixedSingle
            End With
        Next
        pic12(0).Location = New Point(82, 287)
        pic12(1).Location = New Point(134, 287)
        pic12(2).Location = New Point(236, 287)
        pic12(3).Location = New Point(441, 287)
        pic12(4).Location = New Point(493, 287)
        pic12(5).Location = New Point(545, 287)
        pic12(6).Location = New Point(649, 287)

    End Sub
End Class