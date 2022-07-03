Imports System.Net
Imports System.IO
Imports System.Web.Script.Serialization

Public Class Form2
    Private Sub Form2_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Timer1.Enabled = True
        Timer1.Start()
        GET_PENDAFTARAN()
        PANEL_PENDAFTARAN.Visible = True


    End Sub
    Private Sub Timer1_Tick(sender As Object, e As EventArgs) Handles Timer1.Tick
        Timer1.Start()
        Label1.Text = TimeOfDay
        Label2.Text = Today
    End Sub
    Private Sub Btn_minim_Click(sender As Object, e As EventArgs) Handles btn_minim.Click
        Me.WindowState = FormWindowState.Minimized
    End Sub

    Private Sub Btn_close_Click(sender As Object, e As EventArgs) Handles btn_close.Click
        Me.Close()
        Form1.Show()
        Form1.txt_password.Text = ""
        Form1.txt_username.Text = ""
        Form1.txt_username.Focus()
        Form1.txt_show_password.Checked = False
    End Sub

    Private Sub Btn_logout_Click(sender As Object, e As EventArgs) Handles btn_logout.Click

        Dim tanya
        tanya = MessageBox.Show("Anda yakin ingin keluar ??", "Keluar", MessageBoxButtons.YesNo, MessageBoxIcon.Question)
        If tanya = vbYes Then
            Me.Close()
            Form1.txt_password.Text = ""
            Form1.txt_username.Text = ""
            Form1.txt_show_password.Checked = False

        End If

    End Sub
    Private Sub GET_PENDAFTARAN()
        Try
            DGV_PENDAFTARAN.Rows.Clear()
            Dim uriString As String = "http://localhost/ppdb-web/api/Datapendaftaran"
            Dim uri As New Uri(uriString)
            Dim request As HttpWebRequest = HttpWebRequest.Create(uri)
            request.Method = "GET"
            Dim response As HttpWebResponse = request.GetResponse()
            Dim read = New StreamReader(response.GetResponseStream())
            Dim raw As String = read.ReadToEnd()
            Dim dict As Object = New JavaScriptSerializer().Deserialize(Of List(Of Object))(raw)
            For Each item As Object In dict

                DGV_PENDAFTARAN.Rows.Add(item("nisn").ToString, item("fullname").ToString,
                item("asal_sekolah").ToString, item("tempat_lahir").ToString, item("tgl_lahir").ToString, item("alamat").ToString, item("gender").ToString, item("telepon").ToString)
            Next
            Me.DGV_PENDAFTARAN.ColumnHeadersDefaultCellStyle.Font = New Font("Times New Roman", 10)
            Me.DGV_PENDAFTARAN.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill




        Catch ex As Exception
            MsgBox(ex.Message, MsgBoxStyle.Exclamation, "Notifikasi")
        End Try


    End Sub

    Private Sub Btn_panel_data_pendaftaran_Click(sender As Object, e As EventArgs) Handles btn_panel_data_pendaftaran.Click
        PANEL_PENDAFTARAN.Visible = True
        PANEL_VERIFIKASI_SISWA.Visible = False
        PANEL_HAK_ACCESS.Visible = False
        PANEL_HASIL_SELEKSI.Visible = False
        PANEL_JADWAL.Visible = False
        PANEL_USER_GRUP.Visible = False




    End Sub


    '*************************************'

    'FUNCTION CEK NILAI'

    Private Sub CEK_NILAI()
        Try
            DGV_NILAI.Rows.Clear()
            Dim uriString As String = "http://localhost/ppdb-web/api/Datadokument/" + txt_nisnn.Text
            Dim uri As New Uri(uriString)
            Dim request As HttpWebRequest = HttpWebRequest.Create(uri)
            request.Method = "GET"
            Dim response As HttpWebResponse = request.GetResponse()
            Dim read = New StreamReader(response.GetResponseStream())
            Dim raw As String = read.ReadToEnd()
            Dim dict As Object = New JavaScriptSerializer().Deserialize(Of List(Of Object))(raw)
            For Each item As Object In dict
                DGV_NILAI.Rows.Add(item("nisn").ToString, item("fullname").ToString,
                 item("nilai1").ToString, item("nilai2").ToString, item("nilai3").ToString, item("nilai4").ToString, item("nilai5").ToString, item("rata_rata").ToString)


            Next
            Me.DGV_NILAI.ColumnHeadersDefaultCellStyle.Font = New Font("Times New Roman", 10)
            Me.DGV_NILAI.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill


        Catch ex As Exception
            MsgBox(ex.Message, MsgBoxStyle.Exclamation, "Notifikasi")
        End Try


    End Sub

    Private Sub Btn_cek_nilai_Click(sender As Object, e As EventArgs) Handles btn_cek_nilai.Click
        If txt_nisnn.Text = "" Then
            MessageBox.Show("ISI DATA DENGAN BENAR", "Infomation", MessageBoxButtons.OK, MessageBoxIcon.Information)
            txt_nisnn.Focus()

        End If
        CEK_NILAI()

    End Sub


    '*************************************'

    'FUNCTION CEK STATUS'

    Public Sub STATUS_LULUS()
        Try
            DGV_CEK_STATUS.Rows.Clear()
            Dim uriString As String = "http://localhost/ppdb-web/api/verifikasi/" + "2"
            Dim uri As New Uri(uriString)
            Dim request As HttpWebRequest = HttpWebRequest.Create(uri)
            request.Method = "GET"
            Dim response As HttpWebResponse = request.GetResponse()
            Dim read = New StreamReader(response.GetResponseStream())
            Dim raw As String = read.ReadToEnd()
            Dim dict As Object = New JavaScriptSerializer().Deserialize(Of List(Of Object))(raw)
            For Each item As Object In dict
                DGV_CEK_STATUS.Rows.Add(item("nisn").ToString, item("fullname").ToString, item("asal_sekolah").ToString,
                 item("gender").ToString, item("telepon").ToString, item("jurusan").ToString, item("rata_rata").ToString)


            Next
            Me.DGV_CEK_STATUS.ColumnHeadersDefaultCellStyle.Font = New Font("Times New Roman", 10)


        Catch ex As Exception
            MsgBox(ex.Message, MsgBoxStyle.Exclamation, "Notifikasi")
        End Try


    End Sub


    Private Sub Btn_status_Click(sender As Object, e As EventArgs) Handles btn_status.Click
        STATUS_LULUS()

        txt_verifkasi.Text = ""
        txt_verifkasi.Focus()
        DGV_VERIFIKASI.Visible = False
        DGV_CEK_STATUS.Visible = True
        btn_verifikasi.Visible = False
        Guna_verifikasi.Visible = True
        Label_note_verifikasi.Visible = False

        DGV_NILAI.Visible = True
        btn_cek_nilai.Visible = True
        NISN_NILAI_LABEL.Visible = True
        txt_nisnn.Visible = True
        btn_status.Enabled = False
        Panel_NILAI_TERTINGGI.Visible = False



    End Sub
    '*************************************'

    'FUNCTION BY ORDER'



    Public Sub GET_VERIFIKASI_BY_ORDER()

        Dim no As Integer
        Me.DGV_VERIFIKASI.Rows.Clear()
        buka()
        rec.Open("Select * from siswa where rata_rata like '%" & txt_nilai_tertinggi.Text & "%' order by rata_rata DESC", con, 3, 2)
        no = 1
        Do While Not rec.EOF

            Me.DGV_VERIFIKASI.Rows.Add(rec.Fields("nisn").Value, rec.Fields("fullname").Value, rec.Fields("nilai1").Value, rec.Fields("nilai2").Value, rec.Fields("nilai3").Value, rec.Fields("nilai4").Value, rec.Fields("nilai5").Value, rec.Fields("rata_rata").Value)
            rec.MoveNext()
            no = no + 1
        Loop
        tutup()



    End Sub


    Private Sub Btn_cek_nilai_tertinggi_Click(sender As Object, e As EventArgs) Handles btn_cek_nilai_tertinggi.Click
        GET_VERIFIKASI_BY_ORDER()

    End Sub

    '*************************************'

    'FUNCTION VERIFIKASI'

    Private Sub GET_VERIFIKASI()  'GET VERIFIKASI
        Try
            DGV_VERIFIKASI.Rows.Clear()
            Dim uriString As String = "http://localhost/ppdb-web/api/verifikasi/" + "0"
            Dim uri As New Uri(uriString)
            Dim request As HttpWebRequest = HttpWebRequest.Create(uri)
            request.Method = "GET"
            Dim response As HttpWebResponse = request.GetResponse()
            Dim read = New StreamReader(response.GetResponseStream())
            Dim raw As String = read.ReadToEnd()
            Dim dict As Object = New JavaScriptSerializer().Deserialize(Of List(Of Object))(raw)
            For Each item As Object In dict
                DGV_VERIFIKASI.Rows.Add(item("nisn").ToString, item("fullname").ToString,
                 item("nilai1").ToString, item("nilai2").ToString, item("nilai3").ToString, item("nilai4").ToString, item("nilai5").ToString, item("rata_rata").ToString)


            Next
            Me.DGV_VERIFIKASI.ColumnHeadersDefaultCellStyle.Font = New Font("Times New Roman", 10)
            Me.DGV_VERIFIKASI.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill


        Catch ex As Exception
            MsgBox(ex.Message, MsgBoxStyle.Exclamation, "Notifikasi")
        End Try


    End Sub


    '*******************'

    'TABEL STATUS_SELEKSI'

    Public Sub UPDATE_STATUS_SELEKSI_LULUS()
        Dim uriString As String = "http://localhost/ppdb-web/api/verifikasi/" + txt_verifkasi.Text + "/" + "2"
        Dim uri As New Uri(uriString)
        Dim request As HttpWebRequest = HttpWebRequest.Create(uri)
        request.Method = "PUT"
        Dim response As HttpWebResponse = request.GetResponse()
        Dim read = New StreamReader(response.GetResponseStream())
        Dim raw As String = read.ReadToEnd()
        MessageBox.Show("UPDATE SELEKSI LULUS BERHASIL", "Infomation", MessageBoxButtons.OK, MessageBoxIcon.Information)
        txt_verifkasi.Text = ""
        GET_VERIFIKASI()


    End Sub

    Public Sub UPDATE_STATUS_SELEKSI_TIDAK_LULUS()
        Dim uriString As String = "http://localhost/ppdb-web/api/verifikasi/" + txt_verifkasi.Text + "/" + "1"
        Dim uri As New Uri(uriString)
        Dim request As HttpWebRequest = HttpWebRequest.Create(uri)
        request.Method = "PUT"
        Dim response As HttpWebResponse = request.GetResponse()
        Dim read = New StreamReader(response.GetResponseStream())
        Dim raw As String = read.ReadToEnd()
        MessageBox.Show("UPDATE TIDAK LULUS BERHASIL", "Infomation", MessageBoxButtons.OK, MessageBoxIcon.Information)

        txt_verifkasi.Text = ""
        GET_VERIFIKASI()

    End Sub


    '*******************'

    'TABEL STATUS_VERIFIKASI'

    Public Sub UPDATE_STATUS_VERIFIKASI_LULUS()
        Dim uriString As String = "http://localhost/ppdb-web/api/status_verifikasi/" + txt_verifkasi.Text + "/" + "2"
        Dim uri As New Uri(uriString)
        Dim request As HttpWebRequest = HttpWebRequest.Create(uri)
        request.Method = "PUT"
        Dim response As HttpWebResponse = request.GetResponse()
        Dim read = New StreamReader(response.GetResponseStream())
        Dim raw As String = read.ReadToEnd()

    End Sub

    Public Sub UPDATE_STATUS_VERIFIKASI_TIDAK_LULUS()
        Dim uriString As String = "http://localhost/ppdb-web/api/status_verifikasi/" + txt_verifkasi.Text + "/" + "1"
        Dim uri As New Uri(uriString)
        Dim request As HttpWebRequest = HttpWebRequest.Create(uri)
        request.Method = "PUT"
        Dim response As HttpWebResponse = request.GetResponse()
        Dim read = New StreamReader(response.GetResponseStream())
        Dim raw As String = read.ReadToEnd()

    End Sub

    '****************************'

    Private Sub Btn_verifikasi_Click(sender As Object, e As EventArgs) Handles btn_verifikasi.Click
        If txt_verifkasi.Text = "" Then
            MessageBox.Show("DATA TIDAK BOLEH KOSONG", "INFORMATION", MessageBoxButtons.OK, MessageBoxIcon.Warning)
        Else
            Dim tanya
            tanya = MessageBox.Show("YES untuk Lolos seleksi, NO untuk tidak lolos seleksi ! ", "WARNING !!!", MessageBoxButtons.YesNo, MessageBoxIcon.Question)

            If tanya = vbYes Then
                btn_status.Enabled = True
                UPDATE_STATUS_SELEKSI_LULUS()

            ElseIf tanya = vbNo Then
                btn_status.Enabled = True
                MessageBox.Show("TIDAK LULUS MAKA TIDAK MASUK TAHAP VERIFIKASI", "Infomation", MessageBoxButtons.OK, MessageBoxIcon.Information)
                UPDATE_STATUS_VERIFIKASI_TIDAK_LULUS()
                UPDATE_STATUS_SELEKSI_TIDAK_LULUS()

            End If

        End If

    End Sub



    Private Sub DGV_VERIFIKASI_CellContentDoubleClick(sender As Object, e As DataGridViewCellEventArgs) Handles DGV_VERIFIKASI.CellContentDoubleClick
        txt_verifkasi.Text = DGV_VERIFIKASI.Rows(e.RowIndex).Cells(0).Value
        txt_nilai_tertinggi.Text = ""
    End Sub

    Private Sub DGV_CEK_STATUS_CellContentClick(sender As Object, e As DataGridViewCellEventArgs) Handles DGV_CEK_STATUS.CellContentClick
        txt_verifkasi.Text = DGV_CEK_STATUS.Rows(e.RowIndex).Cells(0).Value

    End Sub


    Private Sub Btn_panel_verifikasi_Click(sender As Object, e As EventArgs) Handles btn_panel_verifikasi.Click
        PANEL_VERIFIKASI_SISWA.Visible = True
        PANEL_PENDAFTARAN.Visible = False
        PANEL_HASIL_SELEKSI.Visible = False
        PANEL_JADWAL.Visible = False
        PANEL_USER_GRUP.Visible = False
        Panel_NILAI_TERTINGGI.Visible = True


        DGV_VERIFIKASI.Visible = True
        DGV_CEK_STATUS.Visible = False
        DGV_NILAI.Visible = False

        Label_note_verifikasi.Visible = True
        btn_cek_nilai.Visible = False
        NISN_NILAI_LABEL.Visible = False
        txt_nisnn.Visible = False
        btn_verifikasi.Visible = True
        Guna_verifikasi.Visible = False
        btn_status.Enabled = True


        txt_nisnn.Text = ""
        txt_verifkasi.Text = ""
        txt_nilai_tertinggi.Text = ""

        txt_nilai_tertinggi.Focus()
        GET_VERIFIKASI()
        GET_VERIFIKASI_BY_ORDER()



    End Sub




    '*************************************'

    'FUNCTION HASIL SELEKSI'


    Private Sub DATA_HASIL_SELEKSI()
        Try
            DGV_HASIL_SELEKSI.Rows.Clear()
            Dim uriString As String = "http://localhost/ppdb-web/api/datahasilseleksi/"
            Dim uri As New Uri(uriString)
            Dim request As HttpWebRequest = HttpWebRequest.Create(uri)
            request.Method = "GET"
            Dim response As HttpWebResponse = request.GetResponse()
            Dim read = New StreamReader(response.GetResponseStream())
            Dim raw As String = read.ReadToEnd()
            Dim dict As Object = New JavaScriptSerializer().Deserialize(Of List(Of Object))(raw)
            For Each item As Object In dict

                DGV_HASIL_SELEKSI.Rows.Add(item("nisn").ToString, item("fullname").ToString,
                item("asal_sekolah").ToString, item("jurusan").ToString, item("rata_rata").ToString, item("role").ToString)
            Next
            Me.DGV_HASIL_SELEKSI.ColumnHeadersDefaultCellStyle.Font = New Font("Times New Roman", 10)



        Catch ex As Exception
            MsgBox(ex.Message, MsgBoxStyle.Exclamation, "Notifikasi")
        End Try

    End Sub

    Private Sub Btn_hasil_seleksi_Click(sender As Object, e As EventArgs) Handles btn_hasil_seleksi.Click
        PANEL_HASIL_SELEKSI.Visible = True
        PANEL_PENDAFTARAN.Visible = False
        PANEL_VERIFIKASI_SISWA.Visible = False
        PANEL_JADWAL.Visible = False
        PANEL_HAK_ACCESS.Visible = False
        PANEL_USER_GRUP.Visible = False



        DATA_HASIL_SELEKSI()
    End Sub






    '*************************************'

    'FUNCTION HAK ACCES'

    Private Sub GET_DATA_HAK_AKSES()
        Try
            DGV_HAK_ACCES.Rows.Clear()
            Dim uriString As String = "http://localhost/ppdb-web/api/akses/"
            Dim uri As New Uri(uriString)
            Dim request As HttpWebRequest = HttpWebRequest.Create(uri)
            request.Method = "GET"
            Dim response As HttpWebResponse = request.GetResponse()
            Dim read = New StreamReader(response.GetResponseStream())
            Dim raw As String = read.ReadToEnd()
            Dim dict As Object = New JavaScriptSerializer().Deserialize(Of List(Of Object))(raw)
            For Each item As Object In dict

                DGV_HAK_ACCES.Rows.Add(item("username").ToString, item("password").ToString,
                item("status").ToString, item("role").ToString)
            Next
            Me.DGV_HAK_ACCES.ColumnHeadersDefaultCellStyle.Font = New Font("Times New Roman", 10)
            Me.DGV_HAK_ACCES.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill



        Catch ex As Exception
            MsgBox(ex.Message, MsgBoxStyle.Exclamation, "Notifikasi")
        End Try

    End Sub
    Private Sub GET_DATA_COMBO()
        Try
            Dim uriString As String = "http://localhost/ppdb-web/api/Usergrup/"
            Dim uri As New Uri(uriString)
            Dim request As HttpWebRequest = HttpWebRequest.Create(uri)
            request.Method = "GET"
            Dim response As HttpWebResponse = request.GetResponse()
            Dim read = New StreamReader(response.GetResponseStream())
            Dim raw As String = read.ReadToEnd()
            Dim dict As Object = New JavaScriptSerializer().Deserialize(Of List(Of Object))(raw)
            For Each item As Object In dict

                ComboBox_role.Items.Add(item("id_group").ToString)
            Next


        Catch ex As Exception
            MsgBox(ex.Message, MsgBoxStyle.Exclamation, "Notifikasi")
        End Try

    End Sub
    Private Sub CREATE_DATA_HAK_AKSES()
        Dim uriString As String = "http://localhost/ppdb-web/api/akses/" + txt_username_acces.Text + "/" + txt_password.Text + "/" + txt_status.Text + "/" + ComboBox_role.Text
        Dim uri As New Uri(uriString)
        Dim request As HttpWebRequest = HttpWebRequest.Create(uri)
        request.Method = "POST"
        Dim response As HttpWebResponse = request.GetResponse()
        Dim read = New StreamReader(response.GetResponseStream())
        Dim raw As String = read.ReadToEnd()
        MessageBox.Show("CREATE DATA BERHASIL", "Infomation", MessageBoxButtons.OK, MessageBoxIcon.Information)

        txt_username_acces.Text = ""
        txt_password.Text = ""
        txt_status.Text = ""
        ComboBox_role.Text = ""

    End Sub



    Private Sub UPDATE_DATA_HAK_AKSES()
        Dim uriString As String = "http://localhost/ppdb-web/api/akses/" + txt_username_acces.Text + "/" + txt_password.Text + "/" + txt_status.Text + "/" + ComboBox_role.Text
        Dim uri As New Uri(uriString)
        Dim request As HttpWebRequest = HttpWebRequest.Create(uri)
        request.Method = "PUT"
        Dim response As HttpWebResponse = request.GetResponse()
        Dim read = New StreamReader(response.GetResponseStream())
        Dim raw As String = read.ReadToEnd()
        MessageBox.Show("UPDATE DATA BERHASIL", "Infomation", MessageBoxButtons.OK, MessageBoxIcon.Information)

        txt_username_acces.Text = ""
        txt_password.Text = ""
        txt_status.Text = ""
        ComboBox_role.Text = ""
        txt_status.Enabled = True
        txt_delete.Enabled = True
        txt_username_acces.Focus()



    End Sub


    Private Sub DELETE_DATA_HAK_AKSES()
        Dim del As String

        del = txt_delete.Text

        Try

            Dim uriString As String = "http://localhost/ppdb-web/api/akses/" + del
            Dim uri As New Uri(uriString)
            Dim request As HttpWebRequest = HttpWebRequest.Create(uri)
            request.Method = "DELETE"
            Dim response As HttpWebResponse = request.GetResponse()
            Dim read = New StreamReader(response.GetResponseStream())
            Dim raw As String = read.ReadToEnd()
            MessageBox.Show("Delete Berhasil", "Infomation", MessageBoxButtons.OK, MessageBoxIcon.Information)
            txt_delete.Text = ""
            btn_create.Enabled = True
            btn_update.Enabled = True
            txt_username_acces.Enabled = True



        Catch ex As Exception
            MsgBox(ex.Message, MsgBoxStyle.Exclamation, "Notifikasi")
        End Try

    End Sub

    Private Sub Btn_delete_Click(sender As Object, e As EventArgs) Handles btn_delete.Click
        If txt_delete.Text = "" Then
            MessageBox.Show("USERNAME TIDAK BOLEH KOSONG", "INFORMATION", MessageBoxButtons.OK, MessageBoxIcon.Warning)
        Else
            Dim tanya
            tanya = MessageBox.Show("Apakah anda ingin delete data ?", "WARNING !!!", MessageBoxButtons.YesNo, MessageBoxIcon.Question)

            If tanya = vbYes Then
                DELETE_DATA_HAK_AKSES()
                GET_DATA_HAK_AKSES()
                btn_create.Enabled = True

            End If

        End If



    End Sub

    Private Sub Btn_create_Click(sender As Object, e As EventArgs) Handles btn_create.Click
        If txt_status.Text = "" Then
            MessageBox.Show("ISI DATA DENGAN BENAR", "Infomation", MessageBoxButtons.OK, MessageBoxIcon.Information)
        Else
            CREATE_DATA_HAK_AKSES()
            GET_DATA_HAK_AKSES()
        End If

    End Sub

    Private Sub Btn_update_Click(sender As Object, e As EventArgs) Handles btn_update.Click
        If txt_status.Text = "" Then
            MessageBox.Show("ISI DATA DENGAN BENAR", "INFORMATION", MessageBoxButtons.OK, MessageBoxIcon.Information)


        ElseIf ComboBox_role.Text = "" Then
            MessageBox.Show("ROLE TIDAK BOLEH KOSONG", "INFORMATION", MessageBoxButtons.OK, MessageBoxIcon.Information)

        Else
            UPDATE_DATA_HAK_AKSES()
            GET_DATA_HAK_AKSES()
            txt_username_acces.Enabled = True
            btn_create.Enabled = True
            btn_delete.Enabled = True
            txt_username_acces.Focus()
        End If

    End Sub


    Private Sub DGV_HAK_ACCES_CellContentClick(sender As Object, e As DataGridViewCellEventArgs) Handles DGV_HAK_ACCES.CellContentClick
        txt_username_acces.Text = DGV_HAK_ACCES.Rows(e.RowIndex).Cells(0).Value
        txt_password.Text = DGV_HAK_ACCES.Rows(e.RowIndex).Cells(1).Value
        txt_status.Text = DGV_HAK_ACCES.Rows(e.RowIndex).Cells(2).Value
        txt_username_acces.Enabled = False
        btn_create.Enabled = False
        btn_delete.Enabled = False
        btn_update.Enabled = True
        txt_delete.Text = ""
    End Sub

    Private Sub DGV_HAK_ACCES_CellContentDoubleClick(sender As Object, e As DataGridViewCellEventArgs) Handles DGV_HAK_ACCES.CellContentDoubleClick
        txt_delete.Text = DGV_HAK_ACCES.Rows(e.RowIndex).Cells(0).Value
        btn_create.Enabled = False
        btn_update.Enabled = False
        btn_delete.Enabled = True
        txt_username_acces.Text = ""
        txt_password.Text = ""
        txt_status.Text = ""
        ComboBox_role.Text = ""

    End Sub



    Private Sub Btn_hak_acces_Click(sender As Object, e As EventArgs) Handles btn_hak_acces.Click
        PANEL_HAK_ACCESS.Visible = True
        PANEL_PENDAFTARAN.Visible = False
        PANEL_VERIFIKASI_SISWA.Visible = False
        PANEL_HASIL_SELEKSI.Visible = False
        PANEL_JADWAL.Visible = False
        PANEL_USER_GRUP.Visible = False



        GET_DATA_HAK_AKSES()
        ComboBox_role.Items.Clear()
        GET_DATA_COMBO()


    End Sub




    '*************************************'

    'FUNCTION JADWAL'

    Private Sub GET_JADWAL()
        Try
            DGV_JADWAL.Rows.Clear()
            Dim uriString As String = "http://localhost/ppdb-web/api/jadwal/"
            Dim uri As New Uri(uriString)
            Dim request As HttpWebRequest = HttpWebRequest.Create(uri)
            request.Method = "GET"
            Dim response As HttpWebResponse = request.GetResponse()
            Dim read = New StreamReader(response.GetResponseStream())
            Dim raw As String = read.ReadToEnd()
            Dim dict As Object = New JavaScriptSerializer().Deserialize(Of List(Of Object))(raw)
            For Each item As Object In dict

                DGV_JADWAL.Rows.Add(item("id").ToString, item("verifikasi").ToString, item("hasil_seleksi").ToString, item("mulai_pendaftaran").ToString,
                item("akhir_pendaftaran").ToString, item("mulai_verifikasi").ToString, item("pengumuman_hasil_seleksi").ToString)
            Next
            Me.DGV_JADWAL.ColumnHeadersDefaultCellStyle.Font = New Font("Times New Roman", 10)



        Catch ex As Exception
            MsgBox(ex.Message, MsgBoxStyle.Exclamation, "Notifikasi")
        End Try

    End Sub


    Public Sub UPDATE_JADWAL()
        Dim DT_MULAI_PENDAFTARAN_FORMAT As String
        Dim DT_AKHIR_PENDAFTARAN_FORMAT As String
        Dim DT_MULAI_VERIFIKASI_FORMAT As String
        Dim DT_HASIL_VERIFIKASI_FORMAT As String


        DT_MULAI_PENDAFTARAN_FORMAT = Format(DT_MULAI_PENDAFTARAN.Value, "yyyy-MM-dd")
        DT_AKHIR_PENDAFTARAN_FORMAT = Format(DT_AKHIR_PENDAFTARAN.Value, "yyyy-MM-dd")
        DT_MULAI_VERIFIKASI_FORMAT = Format(DT_MULAI_VERIFIKASI.Value, "yyyy-MM-dd")
        DT_HASIL_VERIFIKASI_FORMAT = Format(DT_HASIL_SELEKSI.Value, "yyyy-MM-dd")

        Dim uriString As String = "http://localhost/ppdb-web/api/jadwal/" + "1" + "/" + DT_MULAI_PENDAFTARAN_FORMAT + "/" + DT_AKHIR_PENDAFTARAN_FORMAT + "/" + DT_MULAI_VERIFIKASI_FORMAT + "/" + DT_HASIL_VERIFIKASI_FORMAT
        Dim uri As New Uri(uriString)
        Dim request As HttpWebRequest = HttpWebRequest.Create(uri)
        request.Method = "PUT"
        Dim response As HttpWebResponse = request.GetResponse()
        Dim read = New StreamReader(response.GetResponseStream())
        Dim raw As String = read.ReadToEnd()
        MessageBox.Show("UPDATE DATA BERHASIL", "Infomation", MessageBoxButtons.OK, MessageBoxIcon.Information)




    End Sub

    Private Sub Btn_jadwal_Click(sender As Object, e As EventArgs) Handles btn_jadwal.Click
        PANEL_PENDAFTARAN.Visible = False
        PANEL_VERIFIKASI_SISWA.Visible = False
        PANEL_HASIL_SELEKSI.Visible = False
        PANEL_HAK_ACCESS.Visible = False
        PANEL_JADWAL.Visible = True
        PANEL_USER_GRUP.Visible = False


        GET_JADWAL()

    End Sub

    Private Sub Btn_jadwal_update_Click(sender As Object, e As EventArgs) Handles btn_jadwal_update.Click
        UPDATE_JADWAL()
        GET_JADWAL()
    End Sub






    '*********************************************'


    'FUNCTION USER GROP

    Private Sub GET_DATA_USERGRUP()
        Try
            DGV_USER_GRUP.Rows.Clear()
            Dim uriString As String = "http://localhost/ppdb-web/api/Usergrup/"
            Dim uri As New Uri(uriString)
            Dim request As HttpWebRequest = HttpWebRequest.Create(uri)
            request.Method = "GET"
            Dim response As HttpWebResponse = request.GetResponse()
            Dim read = New StreamReader(response.GetResponseStream())
            Dim raw As String = read.ReadToEnd()
            Dim dict As Object = New JavaScriptSerializer().Deserialize(Of List(Of Object))(raw)
            For Each item As Object In dict
                DGV_USER_GRUP.Rows.Add(item("id_group").ToString, item("nama").ToString)

                btn_update_user_grup.Enabled = False
                btn_delete_user_grup.Enabled = False
            Next

        Catch ex As Exception
            MsgBox(ex.Message, MsgBoxStyle.Exclamation, "Notifikasi")
        End Try

    End Sub
    Private Sub CREATE_DATA_USER_GRUP()
        DGV_USER_GRUP.Rows.Clear()
        Dim uriString As String = "http://localhost/ppdb-web/api/Usergrup/" + txt_id_grup.Text + "/" + txt_nam_grup.Text
        Dim uri As New Uri(uriString)
        Dim request As HttpWebRequest = HttpWebRequest.Create(uri)
        request.Method = "POST"
        Dim response As HttpWebResponse = request.GetResponse()
        Dim read = New StreamReader(response.GetResponseStream())
        Dim raw As String = read.ReadToEnd()
        MessageBox.Show("CREATE DATA BERHASIL", "Infomation", MessageBoxButtons.OK, MessageBoxIcon.Information)
        txt_id_grup.Text = ""
        txt_nam_grup.Text = ""
        txt_id_grup.Focus()
        GET_DATA_USERGRUP()

    End Sub



    Private Sub UPDATE_DATA_USER_GRUP()
        DGV_USER_GRUP.Rows.Clear()
        Dim uriString As String = "http://localhost/ppdb-web/api/Usergrup/" + txt_id_grup.Text + "/" + txt_nam_grup.Text
        Dim uri As New Uri(uriString)
        Dim request As HttpWebRequest = HttpWebRequest.Create(uri)
        request.Method = "PUT"
        Dim response As HttpWebResponse = request.GetResponse()
        Dim read = New StreamReader(response.GetResponseStream())
        Dim raw As String = read.ReadToEnd()
        MessageBox.Show("UPDATE DATA BERHASIL", "Infomation", MessageBoxButtons.OK, MessageBoxIcon.Information)

        txt_id_grup.Text = ""
        txt_nam_grup.Text = ""
        txt_id_grup.Focus()
        GET_DATA_USERGRUP()


    End Sub


    Private Sub DELETE_DATA_USER_GRUP()
        Dim del As String

        del = txt_id_grup.Text

        Try
            DGV_USER_GRUP.Rows.Clear()
            Dim uriString As String = "http://localhost/ppdb-web/api/Usergrup/" + del
            Dim uri As New Uri(uriString)
            Dim request As HttpWebRequest = HttpWebRequest.Create(uri)
            request.Method = "DELETE"
            Dim response As HttpWebResponse = request.GetResponse()
            Dim read = New StreamReader(response.GetResponseStream())
            Dim raw As String = read.ReadToEnd()
            MessageBox.Show("Delete Berhasil", "Infomation", MessageBoxButtons.OK, MessageBoxIcon.Information)
            txt_id_grup.Text = ""
            txt_nam_grup.Text = ""
            GET_DATA_USERGRUP()


        Catch ex As Exception
            MsgBox(ex.Message, MsgBoxStyle.Exclamation, "Notifikasi")
        End Try

    End Sub



    Private Sub Btn_create_user_grup_Click(sender As Object, e As EventArgs) Handles btn_create_user_grup.Click
        If txt_nam_grup.Text = "" Then
            MessageBox.Show("Tidak Boleh Kosong Berhasil", "Infomation", MessageBoxButtons.OK, MessageBoxIcon.Warning)
        Else
            CREATE_DATA_USER_GRUP()
            btn_create_user_grup.Enabled = True
            txt_id_grup.Enabled = True
            txt_nam_grup.Enabled = True


        End If
    End Sub

    Private Sub Btn_update_user_grup_Click(sender As Object, e As EventArgs) Handles btn_update_user_grup.Click
        If txt_nam_grup.Text = "" Then
            MessageBox.Show("Tidak Boleh Kosong Berhasil", "Infomation", MessageBoxButtons.OK, MessageBoxIcon.Warning)
        Else
            UPDATE_DATA_USER_GRUP()
            btn_create_user_grup.Enabled = True
            txt_id_grup.Enabled = True
            txt_nam_grup.Enabled = True


        End If

    End Sub

    Private Sub Btn_delete_user_grup_Click(sender As Object, e As EventArgs) Handles btn_delete_user_grup.Click

        If txt_id_grup.Text = "" Then
            MessageBox.Show("DATA TIDAK BOLEH KOSONG", "INFORMATION", MessageBoxButtons.OK, MessageBoxIcon.Warning)
        Else
            Dim tanya
            tanya = MessageBox.Show("Apakah anda ingin delete data ?", "WARNING !!!", MessageBoxButtons.YesNo, MessageBoxIcon.Question)

            If tanya = vbYes Then
                DELETE_DATA_USER_GRUP()
                btn_create_user_grup.Enabled = True
                txt_id_grup.Enabled = True
                txt_nam_grup.Enabled = True

            End If

        End If




    End Sub

    Private Sub DGV_USER_GRUP_CellContentClick(sender As Object, e As DataGridViewCellEventArgs) Handles DGV_USER_GRUP.CellContentClick
        txt_id_grup.Text = DGV_USER_GRUP.Rows(e.RowIndex).Cells(0).Value
        txt_nam_grup.Text = DGV_USER_GRUP.Rows(e.RowIndex).Cells(1).Value
        btn_create_user_grup.Enabled = False
        btn_delete_user_grup.Enabled = False
        btn_update_user_grup.Enabled = True
        txt_nam_grup.Focus()
        txt_nam_grup.Enabled = True
        txt_id_grup.Enabled = False


    End Sub

    Private Sub DGV_USER_GRUP_CellContentDoubleClick(sender As Object, e As DataGridViewCellEventArgs) Handles DGV_USER_GRUP.CellContentDoubleClick
        txt_nam_grup.Text = DGV_USER_GRUP.Rows(e.RowIndex).Cells(1).Value
        btn_create_user_grup.Enabled = False
        btn_update_user_grup.Enabled = False
        btn_delete_user_grup.Enabled = True
        txt_nam_grup.Enabled = False
    End Sub

    Private Sub Guna2Button1_Click(sender As Object, e As EventArgs) Handles btn_user_grup.Click
        GET_DATA_USERGRUP()
        PANEL_HAK_ACCESS.Visible = False
        PANEL_HASIL_SELEKSI.Visible = False
        PANEL_JADWAL.Visible = False
        PANEL_PENDAFTARAN.Visible = False
        PANEL_VERIFIKASI_SISWA.Visible = False
        PANEL_USER_GRUP.Visible = True
    End Sub

    Private Sub Guna_verifikasi_Click(sender As Object, e As EventArgs) Handles Guna_verifikasi.Click
        If txt_verifkasi.Text = "" Then
            MessageBox.Show("DATA TIDAK BOLEH KOSONG", "INFORMATION", MessageBoxButtons.OK, MessageBoxIcon.Warning)
        Else
            Dim tanya
            tanya = MessageBox.Show("YES untuk luluskan siswa , NO untuk tidak luluskan siswa !", "WARNING !!!", MessageBoxButtons.YesNo, MessageBoxIcon.Question)

            If tanya = vbYes Then
                btn_status.Enabled = True
                UPDATE_STATUS_VERIFIKASI_LULUS()
                MessageBox.Show("VERIFIKASI LULUS BERHASIL", "Infomation", MessageBoxButtons.OK, MessageBoxIcon.Information)
                txt_verifkasi.Text = ""
                txt_verifkasi.Focus()
            ElseIf tanya = vbNo Then
                btn_status.Enabled = True
                UPDATE_STATUS_VERIFIKASI_TIDAK_LULUS()
                UPDATE_STATUS_SELEKSI_TIDAK_LULUS()
                txt_verifkasi.Text = ""
                txt_verifkasi.Focus()
            End If

        End If

    End Sub



End Class