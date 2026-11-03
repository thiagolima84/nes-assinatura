// Gera o logo animado (GIF) e os icones (PNG) da assinatura de e-mail da Nova Era Solution.
// Compilado pelo gerar-imagens.ps1 (Windows PowerShell 5.1 / C# 5).
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.IO;

public static class NesArt
{
    // Paleta da marca
    static readonly Color Navy = ColorTranslator.FromHtml("#0B2545");
    static readonly Color Blue = ColorTranslator.FromHtml("#1E6FB8");
    static readonly Color BladeDark = ColorTranslator.FromHtml("#0D3A6E");
    static readonly Color BladeLight = ColorTranslator.FromHtml("#4AA3F0");
    static readonly Color Grey = ColorTranslator.FromHtml("#434343");
    static readonly Color SoftBlue = ColorTranslator.FromHtml("#E6F0FA");
    static readonly Color ClaudeOrange = ColorTranslator.FromHtml("#D97757");
    static readonly Color ClaudeBg = ColorTranslator.FromHtml("#FBEDE6");

    public delegate void Painter(Graphics g);

    public static void Build(string outDir)
    {
        Directory.CreateDirectory(outDir);
        BuildIcons(outDir);
        BuildLogo(outDir);
    }

    // ------------------------------------------------------------------ helpers

    static Pen P(Color c, float w)
    {
        Pen p = new Pen(c, w);
        p.StartCap = LineCap.Round;
        p.EndCap = LineCap.Round;
        p.LineJoin = LineJoin.Round;
        return p;
    }

    static GraphicsPath RoundRect(float x, float y, float w, float h, float r)
    {
        GraphicsPath gp = new GraphicsPath();
        float d = r * 2;
        gp.AddArc(x, y, d, d, 180, 90);
        gp.AddArc(x + w - d, y, d, d, 270, 90);
        gp.AddArc(x + w - d, y + h - d, d, d, 0, 90);
        gp.AddArc(x, y + h - d, d, d, 90, 90);
        gp.CloseFigure();
        return gp;
    }

    static void Hq(Graphics g)
    {
        g.SmoothingMode = SmoothingMode.AntiAlias;
        g.PixelOffsetMode = PixelOffsetMode.HighQuality;
        g.CompositingQuality = CompositingQuality.HighQuality;
        g.InterpolationMode = InterpolationMode.HighQualityBicubic;
    }

    static Bitmap Downscale(Bitmap src, int w, int h)
    {
        Bitmap dst = new Bitmap(w, h, PixelFormat.Format32bppArgb);
        using (Graphics g = Graphics.FromImage(dst))
        using (ImageAttributes ia = new ImageAttributes())
        {
            Hq(g);
            g.Clear(Color.Transparent);
            ia.SetWrapMode(WrapMode.TileFlipXY);
            g.DrawImage(src, new Rectangle(0, 0, w, h), 0, 0, src.Width, src.Height, GraphicsUnit.Pixel, ia);
        }
        return dst;
    }

    // Desenha num espaco 100x100 com supersampling e salva como PNG transparente.
    static void SaveIcon(string path, int size, Painter paint)
    {
        int ss = size * 4;
        using (Bitmap big = new Bitmap(ss, ss, PixelFormat.Format32bppPArgb))
        {
            using (Graphics g = Graphics.FromImage(big))
            {
                Hq(g);
                g.Clear(Color.Transparent);
                g.ScaleTransform(ss / 100f, ss / 100f);
                paint(g);
            }
            using (Bitmap small = Downscale(big, size, size))
                small.Save(path, ImageFormat.Png);
        }
    }

    static void Circle(Graphics g, Color c)
    {
        using (SolidBrush b = new SolidBrush(c)) g.FillEllipse(b, 1, 1, 98, 98);
    }

    // ------------------------------------------------------------------ icones

    static void BuildIcons(string dir)
    {
        // Contatos: circulo azul + simbolo branco. 18px na assinatura, gerado em 2x.
        const int C = 36;
        SaveIcon(Path.Combine(dir, "icon-email.png"), C, delegate (Graphics g)
        {
            Circle(g, Blue);
            using (GraphicsPath env = RoundRect(26, 32, 48, 36, 5))
            using (Pen p = P(Color.White, 6)) g.DrawPath(p, env);
            using (Pen p = P(Color.White, 6)) g.DrawLines(p, new[] { new PointF(29, 36), new PointF(50, 52), new PointF(71, 36) });
        });

        SaveIcon(Path.Combine(dir, "icon-whatsapp.png"), C, delegate (Graphics g)
        {
            Circle(g, Blue);
            using (Pen p = P(Color.White, 6)) g.DrawEllipse(p, 26, 24, 48, 48);
            using (SolidBrush w = new SolidBrush(Color.White))
                g.FillPolygon(w, new[] { new PointF(28, 58), new PointF(22, 79), new PointF(43, 72) });
            // monofone
            using (Pen p = P(Color.White, 7)) g.DrawArc(p, 37, 17, 44, 44, 102, 66);
            using (Pen p = P(Color.White, 9))
            {
                g.DrawLine(p, Polar(59, 39, 22, 100), Polar(59, 39, 15, 100));
                g.DrawLine(p, Polar(59, 39, 22, 170), Polar(59, 39, 15, 170));
            }
        });

        SaveIcon(Path.Combine(dir, "icon-instagram.png"), C, delegate (Graphics g)
        {
            Circle(g, Blue);
            using (GraphicsPath sq = RoundRect(27, 27, 46, 46, 13))
            using (Pen p = P(Color.White, 6)) g.DrawPath(p, sq);
            using (Pen p = P(Color.White, 6)) g.DrawEllipse(p, 40, 40, 20, 20);
            using (SolidBrush w = new SolidBrush(Color.White)) g.FillEllipse(w, 59, 34, 7, 7);
        });

        SaveIcon(Path.Combine(dir, "icon-site.png"), C, delegate (Graphics g)
        {
            Circle(g, Blue);
            using (Pen p = P(Color.White, 5))
            {
                g.DrawEllipse(p, 27, 27, 46, 46);
                g.DrawEllipse(p, 40, 27, 20, 46);
                g.DrawLine(p, 28, 50, 72, 50);
            }
        });

        SaveIcon(Path.Combine(dir, "icon-endereco.png"), C, delegate (Graphics g)
        {
            Circle(g, Blue);
            using (GraphicsPath pin = new GraphicsPath())
            {
                pin.AddArc(33, 24, 34, 34, 145, 250);
                pin.AddLine(Polar(50, 41, 17, 35), new PointF(50, 78));
                pin.CloseFigure();
                using (SolidBrush w = new SolidBrush(Color.White)) g.FillPath(w, pin);
            }
            using (SolidBrush b = new SolidBrush(Blue)) g.FillEllipse(b, 43, 34, 14, 14);
        });

        // Servicos: circulo azul-claro + simbolo azul. 24px na assinatura, gerado em 2x.
        const int S = 48;
        SaveIcon(Path.Combine(dir, "svc-network.png"), S, delegate (Graphics g)
        {
            Circle(g, SoftBlue);
            using (GraphicsPath scr = RoundRect(22, 27, 56, 38, 5))
            using (Pen p = P(Blue, 5)) g.DrawPath(p, scr);
            using (Pen p = P(Blue, 5)) { g.DrawLine(p, 50, 66, 50, 73); g.DrawLine(p, 38, 75, 62, 75); }
            using (Pen p = P(Blue, 4)) { g.DrawArc(p, 37, 41, 26, 26, 225, 90); g.DrawArc(p, 43, 47, 14, 14, 225, 90); }
            using (SolidBrush b = new SolidBrush(Blue)) g.FillEllipse(b, 47, 54, 6, 6);
        });

        SaveIcon(Path.Combine(dir, "svc-security.png"), S, delegate (Graphics g)
        {
            Circle(g, SoftBlue);
            using (GraphicsPath sh = new GraphicsPath())
            {
                sh.AddLine(50, 21, 74, 30);
                sh.AddLine(74, 30, 74, 49);
                sh.AddBezier(74, 49, 74, 64, 63, 74, 50, 79);
                sh.AddBezier(50, 79, 37, 74, 26, 64, 26, 49);
                sh.AddLine(26, 49, 26, 30);
                sh.CloseFigure();
                using (SolidBrush b = new SolidBrush(Blue)) g.FillPath(b, sh);
            }
            using (Pen p = P(Color.White, 6)) g.DrawLines(p, new[] { new PointF(39, 50), new PointF(47, 58), new PointF(62, 42) });
        });

        SaveIcon(Path.Combine(dir, "svc-camera.png"), S, delegate (Graphics g)
        {
            Circle(g, SoftBlue);
            using (Pen p = P(Blue, 5)) g.DrawLines(p, new[] { new PointF(58, 56), new PointF(58, 68), new PointF(76, 68) });
            using (Pen p = P(Blue, 5)) g.DrawLine(p, 76, 58, 76, 78);
            GraphicsState st = g.Save();
            g.TranslateTransform(50, 46);
            g.RotateTransform(-16);
            using (GraphicsPath body = RoundRect(-28, -12, 50, 24, 7))
            using (SolidBrush b = new SolidBrush(Blue)) g.FillPath(b, body);
            using (SolidBrush w = new SolidBrush(Color.White)) g.FillEllipse(w, -23, -5, 10, 10);
            g.Restore(st);
        });

        SaveIcon(Path.Combine(dir, "svc-cloud.png"), S, delegate (Graphics g)
        {
            Circle(g, SoftBlue);
            using (GraphicsPath cl = new GraphicsPath())
            {
                cl.AddEllipse(22, 46, 28, 28);
                cl.AddEllipse(34, 29, 34, 34);
                cl.AddEllipse(50, 46, 28, 28);
                cl.AddRectangle(new RectangleF(36, 50, 28, 24));
                cl.FillMode = FillMode.Winding;
                using (SolidBrush b = new SolidBrush(Blue)) g.FillPath(b, cl);
            }
        });

        SaveIcon(Path.Combine(dir, "svc-backup.png"), S, delegate (Graphics g)
        {
            Circle(g, SoftBlue);
            using (Pen p = P(Blue, 5)) g.DrawArc(p, 27, 27, 46, 46, 215, 290);
            PointF a = Polar(50, 50, 23, 215);
            double t = 215 * Math.PI / 180;
            PointF d = new PointF((float)Math.Sin(t), (float)-Math.Cos(t));   // sentido anti-horario
            PointF n = new PointF(-d.Y, d.X);
            using (SolidBrush b = new SolidBrush(Blue))
                g.FillPolygon(b, new[] {
                    new PointF(a.X + d.X * 9, a.Y + d.Y * 9),
                    new PointF(a.X + n.X * 8, a.Y + n.Y * 8),
                    new PointF(a.X - n.X * 8, a.Y - n.Y * 8) });
            using (Pen p = P(Blue, 5)) g.DrawLines(p, new[] { new PointF(50, 36), new PointF(50, 51), new PointF(60, 57) });
        });

        SaveIcon(Path.Combine(dir, "svc-software.png"), S, delegate (Graphics g)
        {
            Circle(g, SoftBlue);
            using (Pen p = P(Blue, 6))
            {
                g.DrawLines(p, new[] { new PointF(37, 35), new PointF(24, 50), new PointF(37, 65) });
                g.DrawLines(p, new[] { new PointF(63, 35), new PointF(76, 50), new PointF(63, 65) });
                g.DrawLine(p, 56, 31, 44, 69);
            }
        });

        SaveIcon(Path.Combine(dir, "svc-ai-agents.png"), S, delegate (Graphics g)
        {
            Circle(g, ClaudeBg);
            DrawSpark(g, 50, 50, ClaudeOrange);
        });
    }

    // Faisca no estilo do simbolo do Claude: raios afinados de comprimentos irregulares.
    static void DrawSpark(Graphics g, float cx, float cy, Color c)
    {
        float[] len = { 30, 24, 28, 22, 31, 25, 27, 21, 30, 23, 26, 22 };
        float[] jit = { 0, 4, -3, 2, -2, 5, 1, -4, 3, -1, 4, -2 };
        using (SolidBrush b = new SolidBrush(c))
        {
            for (int i = 0; i < len.Length; i++)
            {
                double a = (i * 30 + jit[i] - 90) * Math.PI / 180;
                double px = -Math.Sin(a), py = Math.Cos(a);
                float baseW = 4.2f, tipW = 2.2f;
                float tx = cx + (float)(Math.Cos(a) * len[i]);
                float ty = cy + (float)(Math.Sin(a) * len[i]);
                g.FillPolygon(b, new[] {
                    new PointF(cx + (float)(px * baseW), cy + (float)(py * baseW)),
                    new PointF(tx + (float)(px * tipW), ty + (float)(py * tipW)),
                    new PointF(tx - (float)(px * tipW), ty - (float)(py * tipW)),
                    new PointF(cx - (float)(px * baseW), cy - (float)(py * baseW)) });
                g.FillEllipse(b, tx - tipW, ty - tipW, tipW * 2, tipW * 2);
            }
            g.FillEllipse(b, cx - 8, cy - 8, 16, 16);
        }
    }

    static PointF Polar(float cx, float cy, float r, double deg)
    {
        double a = deg * Math.PI / 180;
        return new PointF(cx + (float)(Math.Cos(a) * r), cy + (float)(Math.Sin(a) * r));
    }

    // ------------------------------------------------------------------ logo

    // Logo exibido em 200x60; gerado em 2x (400x120) para telas retina.
    const int LW = 400, LH = 120, SS = 3;

    static GraphicsPath novaPath, solutionPath;
    static RectangleF textBounds;

    static void BuildLogo(string dir)
    {
        BuildWordmark();

        List<Bitmap> frames = new List<Bitmap>();
        List<Rectangle> rects = new List<Rectangle>();
        List<int> delays = new List<int>();

        Rectangle full = new Rectangle(0, 0, LW, LH);
        Rectangle symRect = new Rectangle(4, 4, 112, 112);
        Rectangle txtRect = Rectangle.Inflate(Rectangle.Round(textBounds), 4, 4);
        txtRect.Intersect(full);

        // 1) Quadro de repouso (e o que o Outlook classico mostra)
        frames.Add(RenderLogo(0, -1)); rects.Add(full); delays.Add(160);

        // 2) Simbolo gira uma volta com aceleracao e freio
        int spin = 26;
        for (int i = 1; i <= spin; i++)
        {
            double t = (double)i / spin;
            double e = t < 0.5 ? 4 * t * t * t : 1 - Math.Pow(-2 * t + 2, 3) / 2;
            frames.Add(Crop(RenderLogo(e * Math.PI * 2, -1), symRect)); rects.Add(symRect); delays.Add(4);
        }

        // 3) Brilho passando pelo nome
        int shine = 16;
        for (int i = 0; i <= shine; i++)
        {
            double t = (double)i / shine;
            frames.Add(Crop(RenderLogo(0, i == shine ? -1 : t), txtRect)); rects.Add(txtRect); delays.Add(i == shine ? 260 : 4);
        }

        WriteGif(Path.Combine(dir, "logo-animado.gif"), LW, LH, frames, rects, delays);
        using (Bitmap still = RenderLogo(0, -1)) still.Save(Path.Combine(dir, "logo.png"), ImageFormat.Png);
        foreach (Bitmap b in frames) b.Dispose();
    }

    static Bitmap Crop(Bitmap src, Rectangle r)
    {
        Bitmap dst = src.Clone(r, PixelFormat.Format32bppArgb);
        src.Dispose();
        return dst;
    }

    static void BuildWordmark()
    {
        FontFamily heavy;
        try { heavy = new FontFamily("Segoe UI Black"); } catch { heavy = new FontFamily("Arial Black"); }
        FontFamily semi;
        try { semi = new FontFamily("Segoe UI Semibold"); } catch { semi = new FontFamily("Arial"); }

        float left = 128, width = 258;

        GraphicsPath nova = new GraphicsPath();
        nova.AddString("NOVA ERA", heavy, (int)FontStyle.Regular, 100, new PointF(0, 0), StringFormat.GenericTypographic);
        RectangleF nb = nova.GetBounds();
        float s1 = width / nb.Width;

        // SOLUTION com espacamento largo, mesma largura de NOVA ERA
        GraphicsPath sol = new GraphicsPath();
        string word = "SOLUTION";
        float x = 0, gap = 34;
        foreach (char ch in word)
        {
            GraphicsPath gp = new GraphicsPath();
            gp.AddString(ch.ToString(), semi, (int)FontStyle.Regular, 100, new PointF(0, 0), StringFormat.GenericTypographic);
            RectangleF cb = gp.GetBounds();
            using (Matrix m = new Matrix()) { m.Translate(x - cb.Left, 0); gp.Transform(m); }
            sol.AddPath(gp, false);
            x += cb.Width + gap;
            gp.Dispose();
        }
        RectangleF sb = sol.GetBounds();
        float s2 = width / sb.Width;

        float h1 = nb.Height * s1, h2 = sb.Height * s2, spacing = 9;
        float top = (LH - (h1 + spacing + h2)) / 2f;

        using (Matrix m = new Matrix())
        {
            m.Translate(left, top);
            m.Scale(s1, s1);
            m.Translate(-nb.Left, -nb.Top);
            nova.Transform(m);
        }
        using (Matrix m = new Matrix())
        {
            m.Translate(left, top + h1 + spacing);
            m.Scale(s2, s2);
            m.Translate(-sb.Left, -sb.Top);
            sol.Transform(m);
        }
        novaPath = nova;
        solutionPath = sol;
        RectangleF a = nova.GetBounds(), b2 = sol.GetBounds();
        textBounds = RectangleF.Union(a, b2);
    }

    // rot em radianos; shine em [0,1] ou -1 para sem brilho
    static Bitmap RenderLogo(double rot, double shine)
    {
        using (Bitmap big = new Bitmap(LW * SS, LH * SS, PixelFormat.Format32bppPArgb))
        {
            using (Graphics g = Graphics.FromImage(big))
            {
                Hq(g);
                g.Clear(Color.White);
                g.ScaleTransform(SS, SS);
                DrawSymbol(g, 60, 60, 52, rot);

                using (SolidBrush b = new SolidBrush(Navy)) g.FillPath(b, novaPath);
                using (SolidBrush b = new SolidBrush(Grey)) g.FillPath(b, solutionPath);

                if (shine >= 0)
                {
                    float bandW = 70;
                    float cx = (float)(textBounds.Left - bandW + shine * (textBounds.Width + bandW * 2));
                    RectangleF br = new RectangleF(cx - bandW / 2, textBounds.Top - 10, bandW, textBounds.Height + 20);
                    using (LinearGradientBrush lg = new LinearGradientBrush(br, Color.Transparent, Color.Transparent, 20f))
                    {
                        ColorBlend cb = new ColorBlend(3);
                        cb.Colors = new[] { Color.FromArgb(0, 255, 255, 255), Color.FromArgb(170, 140, 200, 255), Color.FromArgb(0, 255, 255, 255) };
                        cb.Positions = new[] { 0f, 0.5f, 1f };
                        lg.InterpolationColors = cb;
                        g.SetClip(novaPath);
                        g.SetClip(solutionPath, CombineMode.Union);
                        g.FillRectangle(lg, br);
                        g.ResetClip();
                    }
                }
            }
            Bitmap outBmp = Downscale(big, LW, LH);
            // achata sobre branco (GIF sem transparencia parcial)
            Bitmap flat = new Bitmap(LW, LH, PixelFormat.Format32bppArgb);
            using (Graphics g = Graphics.FromImage(flat)) { g.Clear(Color.White); g.DrawImage(outBmp, 0, 0); }
            outBmp.Dispose();
            return flat;
        }
    }

    // Simbolo em espiral (diafragma de 6 laminas) com iluminacao fixa vinda de cima/esquerda.
    static void DrawSymbol(Graphics g, float cx, float cy, float R, double rot)
    {
        int n = 6;
        double h = R * 0.30;
        PointF[] V = new PointF[n];
        PointF[] Pt = new PointF[n];
        for (int i = 0; i < n; i++)
        {
            double a = rot + i * Math.PI * 2 / n - Math.PI / 2;
            V[i] = new PointF(cx + (float)(Math.Cos(a) * h), cy + (float)(Math.Sin(a) * h));
        }
        for (int i = 0; i < n; i++)
        {
            PointF a = V[i], b = V[(i + 1) % n];
            double dx = b.X - a.X, dy = b.Y - a.Y, dl = Math.Sqrt(dx * dx + dy * dy);
            dx /= dl; dy /= dl;
            double qx = b.X - cx, qy = b.Y - cy;
            double qd = qx * dx + qy * dy;
            double t = -qd + Math.Sqrt(qd * qd - (qx * qx + qy * qy - R * R));
            Pt[i] = new PointF((float)(b.X + dx * t), (float)(b.Y + dy * t));
        }

        RectangleF circ = new RectangleF(cx - R, cy - R, R * 2, R * 2);
        for (int i = 0; i < n; i++)
        {
            int j = (i + 1) % n;
            double a0 = Math.Atan2(Pt[i].Y - cy, Pt[i].X - cx) * 180 / Math.PI;
            double a1 = Math.Atan2(Pt[j].Y - cy, Pt[j].X - cx) * 180 / Math.PI;
            double sweep = a1 - a0;
            while (sweep < 0) sweep += 360;

            using (GraphicsPath blade = new GraphicsPath())
            {
                blade.AddLine(V[j], Pt[i]);
                blade.AddArc(circ, (float)a0, (float)sweep);
                blade.AddLine(Pt[j], V[j]);
                blade.CloseFigure();

                double mid = (a0 + sweep / 2) * Math.PI / 180;
                double light = (1 + Math.Cos(mid - 225 * Math.PI / 180)) / 2;
                Color c1 = Lerp(BladeDark, BladeLight, light);
                Color c2 = Lerp(BladeDark, BladeLight, light * 0.55);
                RectangleF bb = blade.GetBounds();
                bb.Inflate(1, 1);
                using (LinearGradientBrush lg = new LinearGradientBrush(new PointF(cx, cy),
                           new PointF(cx + (float)(Math.Cos(mid) * R), cy + (float)(Math.Sin(mid) * R)), c2, c1))
                    g.FillPath(lg, blade);
            }
        }

        using (SolidBrush b = new SolidBrush(Navy)) g.FillPolygon(b, V);
        using (Pen p = new Pen(Color.White, 2.2f))
            for (int i = 0; i < n; i++) g.DrawLine(p, V[(i + 1) % n], Pt[i]);
        using (Pen p = new Pen(Color.FromArgb(60, 11, 37, 69), 1.2f)) g.DrawEllipse(p, circ);
    }

    static Color Lerp(Color a, Color b, double t)
    {
        return Color.FromArgb(
            (int)(a.R + (b.R - a.R) * t),
            (int)(a.G + (b.G - a.G) * t),
            (int)(a.B + (b.B - a.B) * t));
    }

    // ------------------------------------------------------------------ GIF

    static void WriteGif(string path, int w, int h, List<Bitmap> frames, List<Rectangle> rects, List<int> delays)
    {
        // Paleta global: as 256 cores mais frequentes (em 5 bits por canal)
        int[] count = new int[32768];
        long[] sr = new long[32768], sg = new long[32768], sb = new long[32768];
        List<int[]> pixelsPerFrame = new List<int[]>();
        foreach (Bitmap f in frames)
        {
            int[] px = Pixels(f);
            pixelsPerFrame.Add(px);
            foreach (int c in px)
            {
                int r = (c >> 16) & 255, gg = (c >> 8) & 255, b = c & 255;
                int k = ((r >> 3) << 10) | ((gg >> 3) << 5) | (b >> 3);
                count[k]++; sr[k] += r; sg[k] += gg; sb[k] += b;
            }
        }
        List<int> keys = new List<int>();
        for (int k = 0; k < 32768; k++) if (count[k] > 0) keys.Add(k);
        keys.Sort(delegate (int a, int b) { return count[b].CompareTo(count[a]); });
        int pn = Math.Min(256, keys.Count);
        byte[] pal = new byte[768];
        int[][] palC = new int[pn][];
        for (int i = 0; i < pn; i++)
        {
            int k = keys[i];
            palC[i] = k == 32767
                ? new[] { 255, 255, 255 }   // branco puro, para o fundo nao virar cinza
                : new[] { (int)(sr[k] / count[k]), (int)(sg[k] / count[k]), (int)(sb[k] / count[k]) };
            pal[i * 3] = (byte)palC[i][0]; pal[i * 3 + 1] = (byte)palC[i][1]; pal[i * 3 + 2] = (byte)palC[i][2];
        }
        int[] map = new int[32768];
        for (int k = 0; k < 32768; k++)
        {
            if (count[k] == 0) continue;
            int r = (int)(sr[k] / count[k]), gg = (int)(sg[k] / count[k]), b = (int)(sb[k] / count[k]);
            int best = 0, bd = int.MaxValue;
            for (int i = 0; i < pn; i++)
            {
                int dr = r - palC[i][0], dg = gg - palC[i][1], db = b - palC[i][2];
                int d = dr * dr * 3 + dg * dg * 4 + db * db * 2;
                if (d < bd) { bd = d; best = i; }
            }
            map[k] = best;
        }

        using (FileStream fs = new FileStream(path, FileMode.Create))
        using (BinaryWriter bw = new BinaryWriter(fs))
        {
            bw.Write(new[] { (byte)'G', (byte)'I', (byte)'F', (byte)'8', (byte)'9', (byte)'a' });
            bw.Write((ushort)w); bw.Write((ushort)h);
            bw.Write((byte)0xF7); bw.Write((byte)0); bw.Write((byte)0);
            bw.Write(pal);
            // repetir para sempre
            bw.Write(new byte[] { 0x21, 0xFF, 0x0B });
            bw.Write(System.Text.Encoding.ASCII.GetBytes("NETSCAPE2.0"));
            bw.Write(new byte[] { 0x03, 0x01, 0x00, 0x00, 0x00 });

            for (int f = 0; f < frames.Count; f++)
            {
                Rectangle r = rects[f];
                bw.Write(new byte[] { 0x21, 0xF9, 0x04, 0x04 });
                bw.Write((ushort)delays[f]);
                bw.Write(new byte[] { 0x00, 0x00 });
                bw.Write((byte)0x2C);
                bw.Write((ushort)r.X); bw.Write((ushort)r.Y); bw.Write((ushort)r.Width); bw.Write((ushort)r.Height);
                bw.Write((byte)0);
                int[] px = pixelsPerFrame[f];
                byte[] idx = new byte[px.Length];
                for (int i = 0; i < px.Length; i++)
                {
                    int c = px[i];
                    int k = ((((c >> 16) & 255) >> 3) << 10) | ((((c >> 8) & 255) >> 3) << 5) | ((c & 255) >> 3);
                    idx[i] = (byte)map[k];
                }
                bw.Write((byte)8);
                byte[] data = Lzw(idx, 8);
                for (int o = 0; o < data.Length; o += 255)
                {
                    int len = Math.Min(255, data.Length - o);
                    bw.Write((byte)len);
                    bw.Write(data, o, len);
                }
                bw.Write((byte)0);
            }
            bw.Write((byte)0x3B);
        }
    }

    static int[] Pixels(Bitmap b)
    {
        BitmapData d = b.LockBits(new Rectangle(0, 0, b.Width, b.Height), ImageLockMode.ReadOnly, PixelFormat.Format32bppArgb);
        int[] px = new int[b.Width * b.Height];
        for (int y = 0; y < b.Height; y++)
            System.Runtime.InteropServices.Marshal.Copy(IntPtr.Add(d.Scan0, y * d.Stride), px, y * b.Width, b.Width);
        b.UnlockBits(d);
        return px;
    }

    static byte[] Lzw(byte[] idx, int minCodeSize)
    {
        int clear = 1 << minCodeSize, eoi = clear + 1;
        List<byte> outp = new List<byte>();
        int bitBuf = 0, bitCnt = 0, codeSize = minCodeSize + 1, next = eoi + 1;
        Dictionary<int, int> dict = new Dictionary<int, int>();

        Action<int> emit = delegate (int code)
        {
            bitBuf |= code << bitCnt;
            bitCnt += codeSize;
            while (bitCnt >= 8) { outp.Add((byte)(bitBuf & 0xFF)); bitBuf >>= 8; bitCnt -= 8; }
        };

        emit(clear);
        int prefix = idx[0];
        for (int i = 1; i < idx.Length; i++)
        {
            int k = idx[i];
            int key = (prefix << 8) | k;
            int code;
            if (dict.TryGetValue(key, out code)) { prefix = code; continue; }
            emit(prefix);
            if (next < 4096)
            {
                dict[key] = next++;
                if (next > (1 << codeSize) && codeSize < 12) codeSize++;
            }
            else
            {
                emit(clear);
                dict.Clear();
                next = eoi + 1;
                codeSize = minCodeSize + 1;
            }
            prefix = k;
        }
        emit(prefix);
        emit(eoi);
        if (bitCnt > 0) outp.Add((byte)(bitBuf & 0xFF));
        return outp.ToArray();
    }
}
