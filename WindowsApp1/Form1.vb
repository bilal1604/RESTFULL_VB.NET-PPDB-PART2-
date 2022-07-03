Imports System.Net
Imports System.IO

Public Class Form1
    Public Sub LOGIN_API()
        Try
            Dim uriString As String = "http://localhost/ppdb-web/api/login/postlogin/" + txt_username.Text + "/" + txt_password.Text
            Dim client As WebClient = New WebClient
            Dim Reader As StreamReader = New StreamReader(client.OpenRead(uriString))
            Dim return_e As String = Reader.ReadToEnd

            If return_e = "BERHASIL LOGIN" Then
                MessageBox.Show(return_e, "Infomation", MessageBoxButtons.OK, MessageBoxIcon.Information)
                If txt_username.Text = "admin" And txt_password.Text = "admin" Then
                    Form2.ShowDialog()
                ElseIf txt_username.Text = "ADMIN" And txt_password.Text = "ADMIN" Then
                    Form2.ShowDialog()
                Else
                    Form2.btn_hak_acces.Visible = False
                    Form2.btn_jadwal.Visible = False
                    Form2.btn_user_grup.Visible = False

                    Form2.ShowDialog()
                End If
            ElseIf return_e = "USERNAME ATAU PASSWORD ANDA SALAH !" Then
                MessageBox.Show(return_e, "Infomation", MessageBoxButtons.OK, MessageBoxIcon.Error)
                txt_username.Text = ""
                txt_password.Text = ""
                txt_username.Focus()

            Else

            End If


        Catch ex As Exception
            MsgBox(ex.Message, MsgBoxStyle.Exclamation, "Notifikasi")
        End Try

    End Sub

    Private Sub Btn_login_Click(sender As Object, e As EventArgs) Handles btn_login.Click

        Form2.btn_hak_acces.Visible = True
        Form2.btn_jadwal.Visible = True
        Form2.btn_user_grup.Visible = True

        If txt_username.Text = "" Then
            MsgBox("Username Tidak Boleh Kosong", MessageBoxIcon.Information)
            txt_username.Focus()

        ElseIf txt_password.Text = "" Then
            MsgBox("Password Tidak Boleh Kosong", MessageBoxIcon.Information)
            txt_password.Focus()
        Else
            LOGIN_API()


        End If

    End Sub

    Private Sub Btn_close_Click(sender As Object, e As EventArgs) Handles btn_close.Click
        Me.Close()

    End Sub

    Private Sub Btn_minim_Click(sender As Object, e As EventArgs) Handles btn_minim.Click
        Me.WindowState = FormWindowState.Minimized

    End Sub
    Private Sub Txt_password_TextChanged(sender As Object, e As EventArgs) Handles txt_password.TextChanged
        txt_password.PasswordChar = "*"
    End Sub

    Private Sub Check_showpassword_CheckedChanged(sender As Object, e As EventArgs) Handles txt_show_password.CheckedChanged

        If txt_show_password.Checked = True Then
            txt_password.PasswordChar = ""
        Else
            txt_password.PasswordChar = "*"
        End If
    End Sub

    Private Sub Btn_batal_Click(sender As Object, e As EventArgs) Handles btn_batal.Click
        txt_password.Text = ""
        txt_username.Text = ""
        txt_username.Focus()
        txt_show_password.Checked = False

    End Sub

    Private Sub Txt_username_TextChanged(sender As Object, e As EventArgs) Handles txt_username.TextChanged
        Form2.Label_username.Text = txt_username.Text

    End Sub
End Class
