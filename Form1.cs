using System;
using System.IO;
using System.Windows.Forms;

namespace worm
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();            
        }

        private void button1_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog1 = new OpenFileDialog();
            openFileDialog1.Filter = "Arquivo AUD|*.aud|Todos os arquivos (*.*)|*.*";
            openFileDialog1.Title = "Escolha um arquivo .AUD...";
            openFileDialog1.Multiselect = true;

            int magic = 0x46464952;
            long WAVEfmt = 0x20746D6645564157;
            long parte1 = 0x0001000100000010;
            long parte2 = 0x0000AC4400005622;
            long parte3 = 0x6174616400100002;
            int tamanho;

            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                foreach (String file in openFileDialog1.FileNames)
                {
                    using (FileStream stream = File.Open(file, FileMode.Open))
                    {
                        BinaryReader br = new BinaryReader(stream);

                        br.BaseStream.Seek(0x48, SeekOrigin.Begin);

                        int quantidadedewav = br.ReadInt32();

                        br.BaseStream.Seek(0x114, SeekOrigin.Begin);

                        int pulo = br.ReadInt32();

                        br.BaseStream.Seek(0x118, SeekOrigin.Begin);

                        int pulo2 = br.ReadInt32();

                        br.BaseStream.Seek(pulo * 4 + pulo2 + 4, SeekOrigin.Current);

                        int verificador = br.ReadInt32();

                        int tamanhoarquivoatual = 0;

                        br.BaseStream.Seek(-8, SeekOrigin.Current);

                        if (verificador == 0x70474156) // Arquivo de PS2 - VAG
                        {
                            for (int i = 1; i <= quantidadedewav + 1; i++)
                            {
                                if (i <= quantidadedewav)
                                {
                                    tamanhoarquivoatual = br.ReadInt32();

                                    tamanho = tamanhoarquivoatual;

                                    byte[] arquivo = br.ReadBytes(tamanhoarquivoatual);

                                    string caminhodapasta = Path.Combine(Path.GetDirectoryName(file), Path.GetFileNameWithoutExtension(file));
                                    Directory.CreateDirectory(caminhodapasta);

                                    string arquivowav = Path.Combine(caminhodapasta, i + ".vag");

                                    using (BinaryWriter bw = new BinaryWriter(File.OpenWrite(arquivowav)))
                                    {
                                        bw.Write(arquivo);
                                    }
                                }
                                else
                                {
                                    tamanhoarquivoatual = br.ReadInt32();

                                    br.BaseStream.Seek(-4, SeekOrigin.Current);

                                    tamanho = tamanhoarquivoatual;

                                    byte[] arquivo = br.ReadBytes(tamanhoarquivoatual);

                                    string caminhodapasta = Path.Combine(Path.GetDirectoryName(file), Path.GetFileNameWithoutExtension(file));
                                    Directory.CreateDirectory(caminhodapasta);

                                    string arquivowav = Path.Combine(caminhodapasta, i + ".bin");

                                    using (BinaryWriter bw = new BinaryWriter(File.OpenWrite(arquivowav)))
                                    {
                                        bw.Write(arquivo);
                                    }
                                }
                            }
                        }
                        else
                        {
                            for (int i = 1; i <= quantidadedewav + 1; i++)
                            {
                                if (i <= quantidadedewav)
                                {
                                    tamanhoarquivoatual = br.ReadInt32();

                                    tamanho = tamanhoarquivoatual;

                                    byte[] arquivo = br.ReadBytes(tamanhoarquivoatual);

                                    string caminhodapasta = Path.Combine(Path.GetDirectoryName(file), Path.GetFileNameWithoutExtension(file));
                                    Directory.CreateDirectory(caminhodapasta);

                                    string arquivowav = Path.Combine(caminhodapasta, i + ".wav");

                                    using (BinaryWriter bw = new BinaryWriter(File.OpenWrite(arquivowav)))
                                    {
                                        bw.Write(magic);
                                        bw.Write(tamanho + 0x24);
                                        bw.Write(WAVEfmt);
                                        bw.Write(parte1);
                                        bw.Write(parte2);
                                        bw.Write(parte3);
                                        bw.Write(tamanho);
                                        bw.Write(arquivo);
                                    }
                                }
                                else
                                {
                                    tamanhoarquivoatual = br.ReadInt32();

                                    br.BaseStream.Seek(-4, SeekOrigin.Current);

                                    tamanho = tamanhoarquivoatual;

                                    byte[] arquivo = br.ReadBytes(tamanhoarquivoatual);

                                    string caminhodapasta = Path.Combine(Path.GetDirectoryName(file), Path.GetFileNameWithoutExtension(file));
                                    Directory.CreateDirectory(caminhodapasta);

                                    string arquivowav = Path.Combine(caminhodapasta, i + ".bin");

                                    using (BinaryWriter bw = new BinaryWriter(File.OpenWrite(arquivowav)))
                                    {
                                        bw.Write(arquivo);
                                    }
                                }
                            }
                        }                        
                        MessageBox.Show("Terminado!", "AVISO!");
                    }
                }
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog1 = new OpenFileDialog();
            openFileDialog1.Filter = "Arquivo AUD|*.aud|Todos os arquivos (*.*)|*.*";
            openFileDialog1.Title = "Escolha um arquivo .AUD...";
            openFileDialog1.Multiselect = true;

            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                foreach (String file in openFileDialog1.FileNames)
                {
                    using (FileStream stream = File.Open(file, FileMode.Open))
                    {
                        string caminhodapasta = Path.Combine(Path.GetDirectoryName(file), Path.GetFileNameWithoutExtension(file));

                        if (Directory.Exists(caminhodapasta))
                        {
                            BinaryReader br = new BinaryReader(stream);
                            BinaryWriter bw = new BinaryWriter(stream);

                            br.BaseStream.Seek(0x48, SeekOrigin.Begin);

                            int quantidadedewav = br.ReadInt32();

                            br.BaseStream.Seek(0x114, SeekOrigin.Begin);

                            int pulo = br.ReadInt32();

                            br.BaseStream.Seek(0x118, SeekOrigin.Begin);

                            int pulo2 = br.ReadInt32();

                            br.BaseStream.Seek(pulo * 4 + pulo2 + 4, SeekOrigin.Current);

                            int verificador = br.ReadInt32();

                            int tamanhoarquivoatual = 0;

                            stream.SetLength(0x800);

                            br.BaseStream.Seek(-8, SeekOrigin.Current);

                            if (verificador == 0x70474156) // Arquivo de PS2 - VAG
                            {
                                for (int i = 1; i <= quantidadedewav + 1; i++)
                                {
                                    if (i <= quantidadedewav)
                                    {
                                        byte[] bytesarquivoaudio = File.ReadAllBytes(Path.Combine(caminhodapasta, i + ".vag"));

                                        int tamanhonovo = bytesarquivoaudio.Length;

                                        bw.Write(tamanhonovo);
                                        bw.Write(bytesarquivoaudio);

                                    }
                                    else
                                    {
                                        byte[] bytesarquivoaudio = File.ReadAllBytes(Path.Combine(caminhodapasta, i + ".bin"));

                                        bw.Write(bytesarquivoaudio);
                                    }
                                }
                            }
                            else // Arquivo PC
                            {
                                for (int i = 1; i <= quantidadedewav + 1; i++)
                                {
                                    if (i <= quantidadedewav)
                                    {
                                        byte[] bytesarquivoaudio = File.ReadAllBytes(Path.Combine(caminhodapasta, i + ".wav"));

                                        int novoTamanho = bytesarquivoaudio.Length - 0x2C;
                                        byte[] novoArray = new byte[novoTamanho];
                                        Array.Copy(bytesarquivoaudio, 0x2C, novoArray, 0, novoTamanho);

                                        bw.Write(novoTamanho);
                                        bw.Write(novoArray);
                                    }
                                    else
                                    {
                                        byte[] bytesarquivo = File.ReadAllBytes(Path.Combine(caminhodapasta, i + ".bin"));

                                        bw.Write(bytesarquivo);
                                    }
                                }
                            }
                            MessageBox.Show("Terminado!", "AVISO!");
                        }
                        else
                        {
                            MessageBox.Show("A pasta \"" + Path.GetFileNameWithoutExtension(file) + "\" não foi encontrada", "AVISO!");
                            return;
                        }
                    }
                }
            }
        }
    }
}
