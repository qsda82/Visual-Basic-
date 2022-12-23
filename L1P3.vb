Public Class L1P3
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
            P.Top = 617 '調整球拍的y座標
            P.Left = (Me.ClientSize.Width - P.Width) / 2 '球拍水平置中
            spacecheck = True
            New_ball()
        ElseIf Dietimes = 2 Then
            Label6.Image = Nothing
            Label5.Image = Nothing
            MsgBox("你死了，還剩1次機會")
            P.Top = 617 '調整球拍的y座標
            P.Left = (Me.ClientSize.Width - P.Width) / 2 '球拍水平置中
            spacecheck = True
            New_ball()
        ElseIf Dietimes = 3 Then
            Label6.Image = Nothing
            Label5.Image = Nothing
            Label4.Image = Nothing
            YOUDIE.Button1.Tag = 3
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
            If check = 204 Then

                Timer1.Stop()
                PASS.Button1.Tag = 3
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
        vy(ball_num) = 3
        ball(ball_num) = New Label()
        With ball(ball_num)
            .Location = New Point(P.Location.X + P.Size.Width / 2 - 15, 585)
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
        P.Top = 617 '調整球拍的y座標
        P.Left = (Me.ClientSize.Width - P.Width) / 2 '球拍水平置中
        P.Width = 131
        spacecheck = True
        New_ball()
        Dim i As Integer
        Dim picy1(2) As PictureBox
        For i = 0 To 1
            If picy1(i) Is Nothing Then
                picy1(i) = New PictureBox
                Me.Controls.Add(picy1(i))
            End If

            With picy1(i)
                .Enabled = True
                .Location = New Point(107 + (i * 52), 56)
                .Size = New Size(51, 20)
                .BackColor = Color.Goldenrod
                .BorderStyle = BorderStyle.FixedSingle
            End With
        Next

        Dim picy2(4) As PictureBox
        For i = 0 To 3
            If picy2(i) Is Nothing Then
                picy2(i) = New PictureBox
                Me.Controls.Add(picy2(i))
            End If

            With picy2(i)
                .Enabled = True
                .Location = New Point(55 + (i * 52), 77)
                .Size = New Size(51, 20)
                .BackColor = Color.Goldenrod
                .BorderStyle = BorderStyle.FixedSingle
            End With
        Next

        Dim picy3(6) As PictureBox
        For i = 0 To 5
            If picy3(i) Is Nothing Then
                picy3(i) = New PictureBox
                Me.Controls.Add(picy3(i))
            End If

            With picy3(i)
                .Enabled = True
                .Location = New Point(3 + (i * 52), 98)
                .Size = New Size(51, 20)
                .BackColor = Color.Goldenrod
                .BorderStyle = BorderStyle.FixedSingle
            End With
        Next

        Dim picy4(6) As PictureBox
        For i = 0 To 5
            If picy4(i) Is Nothing Then
                picy4(i) = New PictureBox
                Me.Controls.Add(picy4(i))
            End If

            With picy4(i)
                .Enabled = True
                .Location = New Point(3 + (i * 52), 119)
                .Size = New Size(51, 20)
                .BackColor = Color.Goldenrod
                .BorderStyle = BorderStyle.FixedSingle
            End With
        Next

        Dim picy5(6) As PictureBox
        For i = 0 To 5
            If picy5(i) Is Nothing Then
                picy5(i) = New PictureBox
                Me.Controls.Add(picy5(i))
            End If

            With picy5(i)
                .Enabled = True
                .Location = New Point(3 + (i * 52), 140)
                .Size = New Size(51, 20)
                .BackColor = Color.Goldenrod
                .BorderStyle = BorderStyle.FixedSingle
            End With
        Next

        Dim picy6(6) As PictureBox
        For i = 0 To 5
            If picy6(i) Is Nothing Then
                picy6(i) = New PictureBox
                Me.Controls.Add(picy6(i))
            End If

            With picy6(i)
                .Enabled = True
                .Location = New Point(55 + (i * 52), 161)
                .Size = New Size(51, 20)
                .BackColor = Color.Goldenrod
                .BorderStyle = BorderStyle.FixedSingle
            End With
        Next

        Dim picy7(4) As PictureBox
        For i = 0 To 3
            If picy7(i) Is Nothing Then
                picy7(i) = New PictureBox
                Me.Controls.Add(picy7(i))
            End If

            With picy7(i)
                .Enabled = True
                .Location = New Point(159 + (i * 52), 182)
                .Size = New Size(51, 20)
                .BackColor = Color.Goldenrod
                .BorderStyle = BorderStyle.FixedSingle
            End With
        Next

        Dim picy8(3) As PictureBox
        For i = 0 To 2
            If picy8(i) Is Nothing Then
                picy8(i) = New PictureBox
                Me.Controls.Add(picy8(i))
            End If

            With picy8(i)
                .Enabled = True
                .Location = New Point(211 + (i * 52), 203)
                .Size = New Size(51, 20)
                .BackColor = Color.Goldenrod
                .BorderStyle = BorderStyle.FixedSingle
            End With
        Next

        Dim picy9(4) As PictureBox
        For i = 0 To 3
            If picy9(i) Is Nothing Then
                picy9(i) = New PictureBox
                Me.Controls.Add(picy9(i))
            End If

            With picy9(i)
                .Enabled = True
                .Location = New Point(159 + (i * 52), 224)
                .Size = New Size(51, 20)
                .BackColor = Color.Goldenrod
                .BorderStyle = BorderStyle.FixedSingle
            End With
        Next

        Dim picy10(6) As PictureBox
        For i = 0 To 5
            If picy10(i) Is Nothing Then
                picy10(i) = New PictureBox
                Me.Controls.Add(picy10(i))
            End If

            With picy10(i)
                .Enabled = True
                .Location = New Point(55 + (i * 52), 245)
                .Size = New Size(51, 20)
                .BackColor = Color.Goldenrod
                .BorderStyle = BorderStyle.FixedSingle
            End With
        Next

        Dim picy11(6) As PictureBox
        For i = 0 To 5
            If picy11(i) Is Nothing Then
                picy11(i) = New PictureBox
                Me.Controls.Add(picy11(i))
            End If

            With picy11(i)
                .Enabled = True
                .Location = New Point(3 + (i * 52), 266)
                .Size = New Size(51, 20)
                .BackColor = Color.Goldenrod
                .BorderStyle = BorderStyle.FixedSingle
            End With
        Next

        Dim picy12(6) As PictureBox
        For i = 0 To 5
            If picy12(i) Is Nothing Then
                picy12(i) = New PictureBox
                Me.Controls.Add(picy12(i))
            End If

            With picy12(i)
                .Enabled = True
                .Location = New Point(3 + (i * 52), 287)
                .Size = New Size(51, 20)
                .BackColor = Color.Goldenrod
                .BorderStyle = BorderStyle.FixedSingle
            End With
        Next

        Dim picy13(6) As PictureBox
        For i = 0 To 5
            If picy13(i) Is Nothing Then
                picy13(i) = New PictureBox
                Me.Controls.Add(picy13(i))
            End If

            With picy13(i)
                .Enabled = True
                .Location = New Point(3 + (i * 52), 308)
                .Size = New Size(51, 20)
                .BackColor = Color.Goldenrod
                .BorderStyle = BorderStyle.FixedSingle
            End With
        Next

        Dim picy14(4) As PictureBox
        For i = 0 To 3
            If picy14(i) Is Nothing Then
                picy14(i) = New PictureBox
                Me.Controls.Add(picy14(i))
            End If

            With picy14(i)
                .Enabled = True
                .Location = New Point(55 + (i * 52), 329)
                .Size = New Size(51, 20)
                .BackColor = Color.Goldenrod
                .BorderStyle = BorderStyle.FixedSingle
            End With
        Next

        Dim picy15(2) As PictureBox
        For i = 0 To 1
            If picy15(i) Is Nothing Then
                picy15(i) = New PictureBox
                Me.Controls.Add(picy15(i))
            End If

            With picy15(i)
                .Enabled = True
                .Location = New Point(107 + (i * 52), 350)
                .Size = New Size(51, 20)
                .BackColor = Color.Goldenrod
                .BorderStyle = BorderStyle.FixedSingle
            End With
        Next
        '' RED RED RED RED RED RED RED RED RED RED RED RED RED RED RED RED RED RED RED RED RED RED RED RED RED RED
        Dim picr1(4) As PictureBox
        For i = 0 To 3
            If picr1(i) Is Nothing Then
                picr1(i) = New PictureBox
                Me.Controls.Add(picr1(i))
            End If

            With picr1(i)
                .Enabled = True
                .Location = New Point(536 + (i * 52), 56)
                .Size = New Size(51, 20)
                .BackColor = Color.PaleVioletRed
                .BorderStyle = BorderStyle.FixedSingle
            End With
        Next

        Dim picr2(6) As PictureBox
        For i = 0 To 5
            If picr2(i) Is Nothing Then
                picr2(i) = New PictureBox
                Me.Controls.Add(picr2(i))
            End If

            With picr2(i)
                .Enabled = True
                .Location = New Point(484 + (i * 52), 77)
                .Size = New Size(51, 20)
                .BackColor = Color.PaleVioletRed
                .BorderStyle = BorderStyle.FixedSingle
            End With
        Next

        Dim picr3(6) As PictureBox
        For i = 0 To 5
            If picr3(i) Is Nothing Then
                picr3(i) = New PictureBox
                Me.Controls.Add(picr3(i))
            End If

            With picr3(i)
                .Enabled = True
                .Location = New Point(484 + (i * 52), 98)
                .Size = New Size(51, 20)
                .BackColor = Color.PaleVioletRed
                .BorderStyle = BorderStyle.FixedSingle
            End With
        Next

        Dim picr4(8) As PictureBox
        For i = 0 To 7
            If picr4(i) Is Nothing Then
                picr4(i) = New PictureBox
                Me.Controls.Add(picr4(i))
            End If

            With picr4(i)
                .Enabled = True
                .Location = New Point(432 + (i * 52), 119)
                .Size = New Size(51, 20)
                .BackColor = Color.PaleVioletRed
                .BorderStyle = BorderStyle.FixedSingle
            End With
        Next
        picr4(1).BackColor = Color.LightGray
        picr4(5).BackColor = Color.LightGray

        Dim picr5(8) As PictureBox
        For i = 0 To 7
            If picr5(i) Is Nothing Then
                picr5(i) = New PictureBox
                Me.Controls.Add(picr5(i))
            End If

            With picr5(i)
                .Enabled = True
                .Location = New Point(432 + (i * 52), 140)
                .Size = New Size(51, 20)
                .BackColor = Color.PaleVioletRed
                .BorderStyle = BorderStyle.FixedSingle
            End With
        Next
        picr5(0).BackColor = Color.LightGray
        picr5(1).BackColor = Color.LightGray
        picr5(2).BackColor = Color.LightGray
        picr5(4).BackColor = Color.LightGray
        picr5(5).BackColor = Color.LightGray
        picr5(6).BackColor = Color.LightGray

        Dim picr6(8) As PictureBox
        For i = 0 To 7
            If picr6(i) Is Nothing Then
                picr6(i) = New PictureBox
                Me.Controls.Add(picr6(i))
            End If

            With picr6(i)
                .Enabled = True
                .Location = New Point(432 + (i * 52), 161)
                .Size = New Size(51, 20)
                .BackColor = Color.PaleVioletRed
                .BorderStyle = BorderStyle.FixedSingle
            End With
        Next
        picr6(0).BackColor = Color.LightGray
        picr6(1).BackColor = Color.LightGray
        picr6(2).BackColor = Color.LightGray
        picr6(4).BackColor = Color.LightGray
        picr6(5).BackColor = Color.LightGray
        picr6(6).BackColor = Color.LightGray

        Dim picr7(9) As PictureBox
        For i = 0 To 8
            If picr7(i) Is Nothing Then
                picr7(i) = New PictureBox
                Me.Controls.Add(picr7(i))
            End If

            With picr7(i)
                .Enabled = True
                .Location = New Point(380 + (i * 52), 182)
                .Size = New Size(51, 20)
                .BackColor = Color.PaleVioletRed
                .BorderStyle = BorderStyle.FixedSingle
            End With
        Next
        picr7(3).BackColor = Color.LightGray
        picr7(7).BackColor = Color.LightGray
        picr7(1).BackColor = Color.SteelBlue
        picr7(2).BackColor = Color.SteelBlue
        picr7(5).BackColor = Color.SteelBlue
        picr7(6).BackColor = Color.SteelBlue

        Dim picr8(9) As PictureBox
        For i = 0 To 8
            If picr8(i) Is Nothing Then
                picr8(i) = New PictureBox
                Me.Controls.Add(picr8(i))
            End If

            With picr8(i)
                .Enabled = True
                .Location = New Point(380 + (i * 52), 203)
                .Size = New Size(51, 20)
                .BackColor = Color.PaleVioletRed
                .BorderStyle = BorderStyle.FixedSingle
            End With
        Next
        picr8(3).BackColor = Color.LightGray
        picr8(7).BackColor = Color.LightGray
        picr8(1).BackColor = Color.SteelBlue
        picr8(2).BackColor = Color.SteelBlue
        picr8(5).BackColor = Color.SteelBlue
        picr8(6).BackColor = Color.SteelBlue

        Dim picr9(9) As PictureBox
        For i = 0 To 8
            If picr9(i) Is Nothing Then
                picr9(i) = New PictureBox
                Me.Controls.Add(picr9(i))
            End If

            With picr9(i)
                .Enabled = True
                .Location = New Point(380 + (i * 52), 224)
                .Size = New Size(51, 20)
                .BackColor = Color.PaleVioletRed
                .BorderStyle = BorderStyle.FixedSingle
            End With
        Next
        picr9(2).BackColor = Color.LightGray
        picr9(6).BackColor = Color.LightGray

        Dim picr10(9) As PictureBox
        For i = 0 To 8
            If picr10(i) Is Nothing Then
                picr10(i) = New PictureBox
                Me.Controls.Add(picr10(i))
            End If

            With picr10(i)
                .Enabled = True
                .Location = New Point(380 + (i * 52), 245)
                .Size = New Size(51, 20)
                .BackColor = Color.PaleVioletRed
                .BorderStyle = BorderStyle.FixedSingle
            End With
        Next

        Dim picr11(9) As PictureBox
        For i = 0 To 8
            If picr11(i) Is Nothing Then
                picr11(i) = New PictureBox
                Me.Controls.Add(picr11(i))
            End If

            With picr11(i)
                .Enabled = True
                .Location = New Point(380 + (i * 52), 266)
                .Size = New Size(51, 20)
                .BackColor = Color.PaleVioletRed
                .BorderStyle = BorderStyle.FixedSingle
            End With
        Next

        Dim picr12(9) As PictureBox
        For i = 0 To 8
            If picr12(i) Is Nothing Then
                picr12(i) = New PictureBox
                Me.Controls.Add(picr12(i))
            End If

            With picr12(i)
                .Enabled = True
                .Location = New Point(380 + (i * 52), 287)
                .Size = New Size(51, 20)
                .BackColor = Color.PaleVioletRed
                .BorderStyle = BorderStyle.FixedSingle
            End With
        Next

        Dim picr13(9) As PictureBox
        For i = 0 To 8
            If picr13(i) Is Nothing Then
                picr13(i) = New PictureBox
                Me.Controls.Add(picr13(i))
            End If

            With picr13(i)
                .Enabled = True
                .Location = New Point(380 + (i * 52), 308)
                .Size = New Size(51, 20)
                .BackColor = Color.PaleVioletRed
                .BorderStyle = BorderStyle.FixedSingle
            End With
        Next

        Dim picr14(9) As PictureBox
        For i = 0 To 8
            If picr14(i) Is Nothing Then
                picr14(i) = New PictureBox
                Me.Controls.Add(picr14(i))
            End If

            With picr14(i)
                .Enabled = True
                .Location = New Point(380 + (i * 52), 329)
                .Size = New Size(51, 20)
                .BackColor = Color.PaleVioletRed
                .BorderStyle = BorderStyle.FixedSingle
            End With
        Next

        Dim picr15(9) As PictureBox
        For i = 0 To 8
            If picr15(i) Is Nothing Then
                picr15(i) = New PictureBox
                Me.Controls.Add(picr15(i))
            End If

            With picr15(i)
                .Enabled = True
                .Location = New Point(380 + (i * 52), 350)
                .Size = New Size(51, 20)
                .BackColor = Color.PaleVioletRed
                .BorderStyle = BorderStyle.FixedSingle
            End With
        Next

        Dim picr16l(3) As PictureBox
        For i = 0 To 2
            If picr16l(i) Is Nothing Then
                picr16l(i) = New PictureBox
                Me.Controls.Add(picr16l(i))
            End If

            With picr16l(i)
                .Enabled = True
                .Location = New Point(380 + (i * 52), 371)
                .Size = New Size(51, 20)
                .BackColor = Color.PaleVioletRed
                .BorderStyle = BorderStyle.FixedSingle
            End With
        Next

        Dim picr16r(5) As PictureBox
        For i = 0 To 4
            If picr16r(i) Is Nothing Then
                picr16r(i) = New PictureBox
                Me.Controls.Add(picr16r(i))
            End If

            With picr16r(i)
                .Enabled = True
                .Location = New Point(588 + (i * 52), 371)
                .Size = New Size(51, 20)
                .BackColor = Color.PaleVioletRed
                .BorderStyle = BorderStyle.FixedSingle
            End With
        Next

        Dim picr17l(2) As PictureBox
        For i = 0 To 1
            If picr17l(i) Is Nothing Then
                picr17l(i) = New PictureBox
                Me.Controls.Add(picr17l(i))
            End If

            With picr17l(i)
                .Enabled = True
                .Location = New Point(432 + (i * 52), 392)
                .Size = New Size(51, 20)
                .BackColor = Color.PaleVioletRed
                .BorderStyle = BorderStyle.FixedSingle
            End With
        Next

        Dim picr17m(2) As PictureBox
        For i = 0 To 1
            If picr17m(i) Is Nothing Then
                picr17m(i) = New PictureBox
                Me.Controls.Add(picr17m(i))
            End If

            With picr17m(i)
                .Enabled = True
                .Location = New Point(640 + (i * 52), 392)
                .Size = New Size(51, 20)
                .BackColor = Color.PaleVioletRed
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
        LY = rand.Next(440, 577)
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


    Private Sub L1P3_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        New_ball()
        Me.Width = 900
        Me.Height = 680
        Dim i As Integer
        Dim picy1(2) As PictureBox
        For i = 0 To 1
            If picy1(i) Is Nothing Then
                picy1(i) = New PictureBox
                Me.Controls.Add(picy1(i))
            End If

            With picy1(i)
                .Enabled = True
                .Location = New Point(107 + (i * 52), 56)
                .Size = New Size(51, 20)
                .BackColor = Color.Goldenrod
                .BorderStyle = BorderStyle.FixedSingle
            End With
        Next

        Dim picy2(4) As PictureBox
        For i = 0 To 3
            If picy2(i) Is Nothing Then
                picy2(i) = New PictureBox
                Me.Controls.Add(picy2(i))
            End If

            With picy2(i)
                .Enabled = True
                .Location = New Point(55 + (i * 52), 77)
                .Size = New Size(51, 20)
                .BackColor = Color.Goldenrod
                .BorderStyle = BorderStyle.FixedSingle
            End With
        Next

        Dim picy3(6) As PictureBox
        For i = 0 To 5
            If picy3(i) Is Nothing Then
                picy3(i) = New PictureBox
                Me.Controls.Add(picy3(i))
            End If

            With picy3(i)
                .Enabled = True
                .Location = New Point(3 + (i * 52), 98)
                .Size = New Size(51, 20)
                .BackColor = Color.Goldenrod
                .BorderStyle = BorderStyle.FixedSingle
            End With
        Next

        Dim picy4(6) As PictureBox
        For i = 0 To 5
            If picy4(i) Is Nothing Then
                picy4(i) = New PictureBox
                Me.Controls.Add(picy4(i))
            End If

            With picy4(i)
                .Enabled = True
                .Location = New Point(3 + (i * 52), 119)
                .Size = New Size(51, 20)
                .BackColor = Color.Goldenrod
                .BorderStyle = BorderStyle.FixedSingle
            End With
        Next

        Dim picy5(6) As PictureBox
        For i = 0 To 5
            If picy5(i) Is Nothing Then
                picy5(i) = New PictureBox
                Me.Controls.Add(picy5(i))
            End If

            With picy5(i)
                .Enabled = True
                .Location = New Point(3 + (i * 52), 140)
                .Size = New Size(51, 20)
                .BackColor = Color.Goldenrod
                .BorderStyle = BorderStyle.FixedSingle
            End With
        Next

        Dim picy6(6) As PictureBox
        For i = 0 To 5
            If picy6(i) Is Nothing Then
                picy6(i) = New PictureBox
                Me.Controls.Add(picy6(i))
            End If

            With picy6(i)
                .Enabled = True
                .Location = New Point(55 + (i * 52), 161)
                .Size = New Size(51, 20)
                .BackColor = Color.Goldenrod
                .BorderStyle = BorderStyle.FixedSingle
            End With
        Next

        Dim picy7(4) As PictureBox
        For i = 0 To 3
            If picy7(i) Is Nothing Then
                picy7(i) = New PictureBox
                Me.Controls.Add(picy7(i))
            End If

            With picy7(i)
                .Enabled = True
                .Location = New Point(159 + (i * 52), 182)
                .Size = New Size(51, 20)
                .BackColor = Color.Goldenrod
                .BorderStyle = BorderStyle.FixedSingle
            End With
        Next

        Dim picy8(3) As PictureBox
        For i = 0 To 2
            If picy8(i) Is Nothing Then
                picy8(i) = New PictureBox
                Me.Controls.Add(picy8(i))
            End If

            With picy8(i)
                .Enabled = True
                .Location = New Point(211 + (i * 52), 203)
                .Size = New Size(51, 20)
                .BackColor = Color.Goldenrod
                .BorderStyle = BorderStyle.FixedSingle
            End With
        Next

        Dim picy9(4) As PictureBox
        For i = 0 To 3
            If picy9(i) Is Nothing Then
                picy9(i) = New PictureBox
                Me.Controls.Add(picy9(i))
            End If

            With picy9(i)
                .Enabled = True
                .Location = New Point(159 + (i * 52), 224)
                .Size = New Size(51, 20)
                .BackColor = Color.Goldenrod
                .BorderStyle = BorderStyle.FixedSingle
            End With
        Next

        Dim picy10(6) As PictureBox
        For i = 0 To 5
            If picy10(i) Is Nothing Then
                picy10(i) = New PictureBox
                Me.Controls.Add(picy10(i))
            End If

            With picy10(i)
                .Enabled = True
                .Location = New Point(55 + (i * 52), 245)
                .Size = New Size(51, 20)
                .BackColor = Color.Goldenrod
                .BorderStyle = BorderStyle.FixedSingle
            End With
        Next

        Dim picy11(6) As PictureBox
        For i = 0 To 5
            If picy11(i) Is Nothing Then
                picy11(i) = New PictureBox
                Me.Controls.Add(picy11(i))
            End If

            With picy11(i)
                .Enabled = True
                .Location = New Point(3 + (i * 52), 266)
                .Size = New Size(51, 20)
                .BackColor = Color.Goldenrod
                .BorderStyle = BorderStyle.FixedSingle
            End With
        Next

        Dim picy12(6) As PictureBox
        For i = 0 To 5
            If picy12(i) Is Nothing Then
                picy12(i) = New PictureBox
                Me.Controls.Add(picy12(i))
            End If

            With picy12(i)
                .Enabled = True
                .Location = New Point(3 + (i * 52), 287)
                .Size = New Size(51, 20)
                .BackColor = Color.Goldenrod
                .BorderStyle = BorderStyle.FixedSingle
            End With
        Next

        Dim picy13(6) As PictureBox
        For i = 0 To 5
            If picy13(i) Is Nothing Then
                picy13(i) = New PictureBox
                Me.Controls.Add(picy13(i))
            End If

            With picy13(i)
                .Enabled = True
                .Location = New Point(3 + (i * 52), 308)
                .Size = New Size(51, 20)
                .BackColor = Color.Goldenrod
                .BorderStyle = BorderStyle.FixedSingle
            End With
        Next

        Dim picy14(4) As PictureBox
        For i = 0 To 3
            If picy14(i) Is Nothing Then
                picy14(i) = New PictureBox
                Me.Controls.Add(picy14(i))
            End If

            With picy14(i)
                .Enabled = True
                .Location = New Point(55 + (i * 52), 329)
                .Size = New Size(51, 20)
                .BackColor = Color.Goldenrod
                .BorderStyle = BorderStyle.FixedSingle
            End With
        Next

        Dim picy15(2) As PictureBox
        For i = 0 To 1
            If picy15(i) Is Nothing Then
                picy15(i) = New PictureBox
                Me.Controls.Add(picy15(i))
            End If

            With picy15(i)
                .Enabled = True
                .Location = New Point(107 + (i * 52), 350)
                .Size = New Size(51, 20)
                .BackColor = Color.Goldenrod
                .BorderStyle = BorderStyle.FixedSingle
            End With
        Next
        '' RED RED RED RED RED RED RED RED RED RED RED RED RED RED RED RED RED RED RED RED RED RED RED RED RED RED
        Dim picr1(4) As PictureBox
        For i = 0 To 3
            If picr1(i) Is Nothing Then
                picr1(i) = New PictureBox
                Me.Controls.Add(picr1(i))
            End If

            With picr1(i)
                .Enabled = True
                .Location = New Point(536 + (i * 52), 56)
                .Size = New Size(51, 20)
                .BackColor = Color.PaleVioletRed
                .BorderStyle = BorderStyle.FixedSingle
            End With
        Next

        Dim picr2(6) As PictureBox
        For i = 0 To 5
            If picr2(i) Is Nothing Then
                picr2(i) = New PictureBox
                Me.Controls.Add(picr2(i))
            End If

            With picr2(i)
                .Enabled = True
                .Location = New Point(484 + (i * 52), 77)
                .Size = New Size(51, 20)
                .BackColor = Color.PaleVioletRed
                .BorderStyle = BorderStyle.FixedSingle
            End With
        Next

        Dim picr3(6) As PictureBox
        For i = 0 To 5
            If picr3(i) Is Nothing Then
                picr3(i) = New PictureBox
                Me.Controls.Add(picr3(i))
            End If

            With picr3(i)
                .Enabled = True
                .Location = New Point(484 + (i * 52), 98)
                .Size = New Size(51, 20)
                .BackColor = Color.PaleVioletRed
                .BorderStyle = BorderStyle.FixedSingle
            End With
        Next

        Dim picr4(8) As PictureBox
        For i = 0 To 7
            If picr4(i) Is Nothing Then
                picr4(i) = New PictureBox
                Me.Controls.Add(picr4(i))
            End If

            With picr4(i)
                .Enabled = True
                .Location = New Point(432 + (i * 52), 119)
                .Size = New Size(51, 20)
                .BackColor = Color.PaleVioletRed
                .BorderStyle = BorderStyle.FixedSingle
            End With
        Next
        picr4(1).BackColor = Color.LightGray
        picr4(5).BackColor = Color.LightGray

        Dim picr5(8) As PictureBox
        For i = 0 To 7
            If picr5(i) Is Nothing Then
                picr5(i) = New PictureBox
                Me.Controls.Add(picr5(i))
            End If

            With picr5(i)
                .Enabled = True
                .Location = New Point(432 + (i * 52), 140)
                .Size = New Size(51, 20)
                .BackColor = Color.PaleVioletRed
                .BorderStyle = BorderStyle.FixedSingle
            End With
        Next
        picr5(0).BackColor = Color.LightGray
        picr5(1).BackColor = Color.LightGray
        picr5(2).BackColor = Color.LightGray
        picr5(4).BackColor = Color.LightGray
        picr5(5).BackColor = Color.LightGray
        picr5(6).BackColor = Color.LightGray

        Dim picr6(8) As PictureBox
        For i = 0 To 7
            If picr6(i) Is Nothing Then
                picr6(i) = New PictureBox
                Me.Controls.Add(picr6(i))
            End If

            With picr6(i)
                .Enabled = True
                .Location = New Point(432 + (i * 52), 161)
                .Size = New Size(51, 20)
                .BackColor = Color.PaleVioletRed
                .BorderStyle = BorderStyle.FixedSingle
            End With
        Next
        picr6(0).BackColor = Color.LightGray
        picr6(1).BackColor = Color.LightGray
        picr6(2).BackColor = Color.LightGray
        picr6(4).BackColor = Color.LightGray
        picr6(5).BackColor = Color.LightGray
        picr6(6).BackColor = Color.LightGray

        Dim picr7(9) As PictureBox
        For i = 0 To 8
            If picr7(i) Is Nothing Then
                picr7(i) = New PictureBox
                Me.Controls.Add(picr7(i))
            End If

            With picr7(i)
                .Enabled = True
                .Location = New Point(380 + (i * 52), 182)
                .Size = New Size(51, 20)
                .BackColor = Color.PaleVioletRed
                .BorderStyle = BorderStyle.FixedSingle
            End With
        Next
        picr7(3).BackColor = Color.LightGray
        picr7(7).BackColor = Color.LightGray
        picr7(1).BackColor = Color.SteelBlue
        picr7(2).BackColor = Color.SteelBlue
        picr7(5).BackColor = Color.SteelBlue
        picr7(6).BackColor = Color.SteelBlue

        Dim picr8(9) As PictureBox
        For i = 0 To 8
            If picr8(i) Is Nothing Then
                picr8(i) = New PictureBox
                Me.Controls.Add(picr8(i))
            End If

            With picr8(i)
                .Enabled = True
                .Location = New Point(380 + (i * 52), 203)
                .Size = New Size(51, 20)
                .BackColor = Color.PaleVioletRed
                .BorderStyle = BorderStyle.FixedSingle
            End With
        Next
        picr8(3).BackColor = Color.LightGray
        picr8(7).BackColor = Color.LightGray
        picr8(1).BackColor = Color.SteelBlue
        picr8(2).BackColor = Color.SteelBlue
        picr8(5).BackColor = Color.SteelBlue
        picr8(6).BackColor = Color.SteelBlue

        Dim picr9(9) As PictureBox
        For i = 0 To 8
            If picr9(i) Is Nothing Then
                picr9(i) = New PictureBox
                Me.Controls.Add(picr9(i))
            End If

            With picr9(i)
                .Enabled = True
                .Location = New Point(380 + (i * 52), 224)
                .Size = New Size(51, 20)
                .BackColor = Color.PaleVioletRed
                .BorderStyle = BorderStyle.FixedSingle
            End With
        Next
        picr9(2).BackColor = Color.LightGray
        picr9(6).BackColor = Color.LightGray

        Dim picr10(9) As PictureBox
        For i = 0 To 8
            If picr10(i) Is Nothing Then
                picr10(i) = New PictureBox
                Me.Controls.Add(picr10(i))
            End If

            With picr10(i)
                .Enabled = True
                .Location = New Point(380 + (i * 52), 245)
                .Size = New Size(51, 20)
                .BackColor = Color.PaleVioletRed
                .BorderStyle = BorderStyle.FixedSingle
            End With
        Next

        Dim picr11(9) As PictureBox
        For i = 0 To 8
            If picr11(i) Is Nothing Then
                picr11(i) = New PictureBox
                Me.Controls.Add(picr11(i))
            End If

            With picr11(i)
                .Enabled = True
                .Location = New Point(380 + (i * 52), 266)
                .Size = New Size(51, 20)
                .BackColor = Color.PaleVioletRed
                .BorderStyle = BorderStyle.FixedSingle
            End With
        Next

        Dim picr12(9) As PictureBox
        For i = 0 To 8
            If picr12(i) Is Nothing Then
                picr12(i) = New PictureBox
                Me.Controls.Add(picr12(i))
            End If

            With picr12(i)
                .Enabled = True
                .Location = New Point(380 + (i * 52), 287)
                .Size = New Size(51, 20)
                .BackColor = Color.PaleVioletRed
                .BorderStyle = BorderStyle.FixedSingle
            End With
        Next

        Dim picr13(9) As PictureBox
        For i = 0 To 8
            If picr13(i) Is Nothing Then
                picr13(i) = New PictureBox
                Me.Controls.Add(picr13(i))
            End If

            With picr13(i)
                .Enabled = True
                .Location = New Point(380 + (i * 52), 308)
                .Size = New Size(51, 20)
                .BackColor = Color.PaleVioletRed
                .BorderStyle = BorderStyle.FixedSingle
            End With
        Next

        Dim picr14(9) As PictureBox
        For i = 0 To 8
            If picr14(i) Is Nothing Then
                picr14(i) = New PictureBox
                Me.Controls.Add(picr14(i))
            End If

            With picr14(i)
                .Enabled = True
                .Location = New Point(380 + (i * 52), 329)
                .Size = New Size(51, 20)
                .BackColor = Color.PaleVioletRed
                .BorderStyle = BorderStyle.FixedSingle
            End With
        Next

        Dim picr15(9) As PictureBox
        For i = 0 To 8
            If picr15(i) Is Nothing Then
                picr15(i) = New PictureBox
                Me.Controls.Add(picr15(i))
            End If

            With picr15(i)
                .Enabled = True
                .Location = New Point(380 + (i * 52), 350)
                .Size = New Size(51, 20)
                .BackColor = Color.PaleVioletRed
                .BorderStyle = BorderStyle.FixedSingle
            End With
        Next

        Dim picr16l(3) As PictureBox
        For i = 0 To 2
            If picr16l(i) Is Nothing Then
                picr16l(i) = New PictureBox
                Me.Controls.Add(picr16l(i))
            End If

            With picr16l(i)
                .Enabled = True
                .Location = New Point(380 + (i * 52), 371)
                .Size = New Size(51, 20)
                .BackColor = Color.PaleVioletRed
                .BorderStyle = BorderStyle.FixedSingle
            End With
        Next

        Dim picr16r(5) As PictureBox
        For i = 0 To 4
            If picr16r(i) Is Nothing Then
                picr16r(i) = New PictureBox
                Me.Controls.Add(picr16r(i))
            End If

            With picr16r(i)
                .Enabled = True
                .Location = New Point(588 + (i * 52), 371)
                .Size = New Size(51, 20)
                .BackColor = Color.PaleVioletRed
                .BorderStyle = BorderStyle.FixedSingle
            End With
        Next

        Dim picr17l(2) As PictureBox
        For i = 0 To 1
            If picr17l(i) Is Nothing Then
                picr17l(i) = New PictureBox
                Me.Controls.Add(picr17l(i))
            End If

            With picr17l(i)
                .Enabled = True
                .Location = New Point(432 + (i * 52), 392)
                .Size = New Size(51, 20)
                .BackColor = Color.PaleVioletRed
                .BorderStyle = BorderStyle.FixedSingle
            End With
        Next

        Dim picr17m(2) As PictureBox
        For i = 0 To 1
            If picr17m(i) Is Nothing Then
                picr17m(i) = New PictureBox
                Me.Controls.Add(picr17m(i))
            End If

            With picr17m(i)
                .Enabled = True
                .Location = New Point(640 + (i * 52), 392)
                .Size = New Size(51, 20)
                .BackColor = Color.PaleVioletRed
                .BorderStyle = BorderStyle.FixedSingle
            End With
        Next
    End Sub
End Class