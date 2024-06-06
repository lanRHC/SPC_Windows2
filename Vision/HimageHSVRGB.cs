using HalconDotNet;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vision2.vision;

namespace SPC_Windows.SPCFile
{
    public class HimageHSVRGB
    {        /// <summary>
             /// 图像通道分解
             /// </summary>
             /// <param name="hObject"></param>
        private void ImageGray()
        {
            try
            {
                if (!HWindID.IsObjectValided(Gray))
                {
                    HOperatorSet.Rgb1ToGray(Image, out Gray);
                }
            }
            catch (Exception ex)
            {
            }
        }
        public void ImageHSV()
        {
            try
            {

                HOperatorSet.CountChannels(ImageT, out HTuple htcon);
                if (htcon.Length == 0)
                {
                    return;
                }
                ImageCountChannels = htcon.TupleInt();
                if (ImageCountChannels == 3 && !HWindID.IsObjectValided(H))
                {
                    if (!HWindID.IsObjectValided(R))
                    {
                        ImageRGB();
                    }
                    if (H != null)
                        H.Dispose();

                    if (S != null)
                        S.Dispose();
                    if (V != null)
                        V.Dispose();

                    HOperatorSet.TransFromRgb(R, G, B, out H, out S, out V, "hsv");

                }
            }
            catch (Exception ex)
            {
            }
        }
        public void ImageRGB()
        {
            try
            {

                HOperatorSet.CountChannels(ImageT, out HTuple htcon);
                if (htcon.Length != 0)
                {
                    ImageCountChannels = htcon.TupleInt();
                    if (ImageCountChannels == 3 && !HWindID.IsObjectValided(R))
                    {
                        if (R != null)
                            R.Dispose();

                        if (G != null)
                            G.Dispose();

                        if (B != null)
                            B.Dispose();

                        HOperatorSet.Decompose3(ImageT, out R, out G, out B);

                    }
                }
            }
            catch (Exception ex)
            {
            }
        }

        public int ImageCountChannels = 1;
        public HimageHSVRGB()
        {

        }
        public HimageHSVRGB(HObject imaget)
        {
            Image = (imaget);
        }

        public HObject GetImageOBJ(ImageTypeObj imageType)
        {
            if (Image == null)
            {
                return new HObject();
            }
            if (!Image.IsInitialized())
            {
                return new HObject();
            }
            if (ImageT == null)
            {
                ImageT = Image;
                DisposeHSVGRB();
                //ImageGray(Image);
            }
            else
            {
                if (ImageTypeObj.Image3 == imageType)
                {
                    return Image;
                }
                if (!Image.Equals(ImageT))
                {
                    ImageT = Image;
                    //Image = ImageT;
                    HOperatorSet.CountChannels(Image, out HTuple htcont);
                    ImageCountChannels = htcont.TupleInt();
                    DisposeHSVGRB();
                }
            }
            if (ImageTypeObj.Image3 == imageType)
            {
                return Image;
            }
            HOperatorSet.CountChannels(Image, out HTuple htcon);
            if (htcon.Length != 0)
            {
                ImageCountChannels = htcon.TupleInt();
            }
            switch (imageType)
            {
                case ImageTypeObj.Image3:
                    return Image;

                case ImageTypeObj.Gray:
                    ImageGray();
                    return Gray;

                case ImageTypeObj.R:
                    ImageRGB();
                    return R;

                case ImageTypeObj.G:
                    ImageRGB();
                    return G;

                case ImageTypeObj.B:
                    ImageRGB();
                    return B;

                case ImageTypeObj.H:
                    ImageHSV();
                    return H;

                case ImageTypeObj.S:
                    ImageHSV();
                    return S;
                case ImageTypeObj.V:
                    ImageHSV();
                    return V;
            }
            return Image;
        }
        public HObject R = null;

        public HObject G = null;

        public HObject B = null;

        public HObject H = null;

        public HObject S = null;

        public HObject V = null;

        public HObject Gray = null;



        private HObject ImageT;
        public HObject Image
        {
            get
            {
                return image;
            }

            set
            {
                if (image != value)
                {
                    DisposeHSVGRB();
                }
                image = value;
            }
        }
        HObject image = new HObject();
        public void Dispose()
        {
            try
            {
                if (image != null)
                {
                    image.Dispose();
                }

                if (ImageT != null)
                {
                    ImageT.Dispose();
                }

                DisposeHSVGRB();


            }
            catch (Exception)
            {
            }
        }
        public void DisposeHSVGRB()
        {
            try
            {
                if (Gray != null)
                {
                    Gray.Dispose();
                }
                if (R != null)
                {
                    R.Dispose();
                    G.Dispose();
                    B.Dispose();
                }
                if (H != null)
                {
                    H.Dispose();
                    S.Dispose();
                    V.Dispose();
                }
            }
            catch (Exception)
            {
            }
        }
    }

}
