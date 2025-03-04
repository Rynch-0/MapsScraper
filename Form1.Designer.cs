﻿using Newtonsoft.Json;
using System;
using System.Windows.Forms;

namespace MapsScraper
{
    partial class Form1
    {
        /// <summary>
        ///Gerekli tasarımcı değişkeni.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///Kullanılan tüm kaynakları temizleyin.
        /// </summary>
        ///<param name="disposing">yönetilen kaynaklar dispose edilmeliyse doğru; aksi halde yanlış.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer üretilen kod

        /// <summary>
        /// Tasarımcı desteği için gerekli metot - bu metodun 
        ///içeriğini kod düzenleyici ile değiştirmeyin.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.contextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.cmbIl = new System.Windows.Forms.ComboBox();
            this.cmbIlce = new System.Windows.Forms.ComboBox();
            this.txtKeyword = new System.Windows.Forms.TextBox();
            this.btnSearch = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // contextMenuStrip1
            // 
            this.contextMenuStrip1.Name = "contextMenuStrip1";
            this.contextMenuStrip1.Size = new System.Drawing.Size(61, 4);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(242, 43);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(55, 13);
            this.label1.TabIndex = 3;
            this.label1.Text = "İl Seçimi";
            this.label1.Click += new System.EventHandler(this.label1_Click);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(242, 124);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(69, 13);
            this.label2.TabIndex = 5;
            this.label2.Text = "İlçe Seçimi";
            this.label2.Click += new System.EventHandler(this.label2_Click);
            // 
            // cmbIl
            // 
            this.cmbIl.FormattingEnabled = true;
            this.cmbIl.Items.AddRange(new object[] {
            "İstanbul",
            "Ankara",
            "İzmir"});
            this.cmbIl.Location = new System.Drawing.Point(212, 59);
            this.cmbIl.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.cmbIl.Name = "cmbIl";
            this.cmbIl.Size = new System.Drawing.Size(140, 21);
            this.cmbIl.TabIndex = 7;
            this.cmbIl.SelectedIndexChanged += new System.EventHandler(this.cmbIl_SelectedIndexChanged);
            // 
            // cmbIlce
            // 
            this.cmbIlce.FormattingEnabled = true;
            this.cmbIlce.Location = new System.Drawing.Point(212, 140);
            this.cmbIlce.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.cmbIlce.Name = "cmbIlce";
            this.cmbIlce.Size = new System.Drawing.Size(140, 21);
            this.cmbIlce.TabIndex = 8;
            this.cmbIlce.SelectedIndexChanged += new System.EventHandler(this.cmbIlce_SelectedIndexChanged);
            // 
            // txtKeyword
            // 
            this.txtKeyword.Location = new System.Drawing.Point(81, 102);
            this.txtKeyword.Name = "txtKeyword";
            this.txtKeyword.Size = new System.Drawing.Size(100, 20);
            this.txtKeyword.TabIndex = 9;
            // 
            // btnSearch
            // 
            this.btnSearch.Location = new System.Drawing.Point(245, 187);
            this.btnSearch.Name = "btnSearch";
            this.btnSearch.Size = new System.Drawing.Size(75, 23);
            this.btnSearch.TabIndex = 10;
            this.btnSearch.Text = "Arama";
            this.btnSearch.UseVisualStyleBackColor = true;
            this.btnSearch.Click += new System.EventHandler(this.btnSearch_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.ControlDark;
            this.ClientSize = new System.Drawing.Size(595, 405);
            this.Controls.Add(this.btnSearch);
            this.Controls.Add(this.txtKeyword);
            this.Controls.Add(this.cmbIlce);
            this.Controls.Add(this.cmbIl);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            this.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.Name = "Form1";
            this.Text = "Maps Scraper";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        private void label1_Click(object sender, EventArgs e)
        {
            throw new NotImplementedException();
        }

        private void cmbIlce_SelectedIndexChanged(object sender, EventArgs e)
        {
            // Seçilen ilçe
            string selectedDistrict = cmbIlce.SelectedItem?.ToString();

            if (!string.IsNullOrEmpty(selectedDistrict))
            {
                // İlçe seçildiğinde yapılacak işlemler
                MessageBox.Show($"Seçilen ilçe: {selectedDistrict}");

                // Burada istediğiniz işlemleri yapabilirsiniz, örneğin:
                // - Google Places API ile arama yapmak
                // - Verileri filtrelemek
            }
        }


        private async void CmbIl_SelectedIndexChanged(object sender, EventArgs e)
        {
            // Seçilen şehir
            string selectedCity = cmbIl.SelectedItem?.ToString();

            if (!string.IsNullOrEmpty(selectedCity))
            {
                // İlçeleri almak için API URL'sini oluşturuyoruz
                string url = $"https://turkiyeapi.dev/api/v1/provinces/{selectedCity}/districts";

                var data = await FetchDataAsync(url); // Veriyi çekiyoruz

                // Verinin doğru şekilde geldiğinden emin olduktan sonra işlemi yapalım
                if (data != null && data.data != null && data.data.Count > 0)
                {
                    cmbIlce.Items.Clear(); // İlçe listesini temizliyoruz

                    // İlçe isimlerini ComboBox'a ekliyoruz
                    foreach (var item in data.data)
                    {
                        cmbIlce.Items.Add(item.name); // İlçe ismini ekliyoruz
                    }
                }
                else
                {
                    MessageBox.Show("İlçeler yüklenemedi.");
                }
            }
            else
            {
                // Şehir seçilmemişse, ilçeleri temizleyelim
                cmbIlce.Items.Clear();
            }
        }


        private void BtnExportExcel_Click(object sender, EventArgs e)
        {
            throw new NotImplementedException();
        }

        private void label2_Click(object sender, EventArgs e)
        {
            throw new NotImplementedException();
        }

        #endregion
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.ComboBox cmbIl;
        private System.Windows.Forms.ComboBox cmbIlce;
        private System.Windows.Forms.TextBox txtKeyword;
        private System.Windows.Forms.Button btnSearch;
    }
}

