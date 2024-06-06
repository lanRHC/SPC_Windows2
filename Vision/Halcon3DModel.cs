using HalconDotNet;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vision2.vision;

namespace SPC_Windows.SPCFile
{
    [JsonObject(MemberSerialization.OptOut)]
    public class Halcon3DModel
    {
        public Halcon3DModel()
        {

            StartTime = DateTime.Now;
        }
        public double MinZ { get; set; }
        ~Halcon3DModel()
        {
            //Dispose();
        }
        public DateTime StartTime { get; set; }
        [JsonIgnore]
        public HObject Image3D { get; set; }
        [JsonIgnore]
        public HObject Image2D { get; set; }
        [JsonIgnore]
        public HObject Image3DMirror { get; set; }


        [JsonIgnore]
        public HTuple Model3D { get; set; }
        /// <summary>
        /// 返回3D彩虹图
        /// </summary>
        /// <returns></returns>
        public HObject GetRainbowChartImage()
        {
            try
            {
                //HOperatorSet.Threshold(Image3D, out HObject hObject, -32767, 99999);
                //HOperatorSet.ReduceDomain(Image3D,  hObject, out HObject imageReduced);
                HOperatorSet.MinMaxGray(Image3D, Image3D, 0, out HTuple min, out HTuple max, out HTuple range);
                HOperatorSet.ScaleImage(Image3D, out HObject hObject1, 255 / (max - min), (-min) * 255 / (max - min));
                HOperatorSet.ConvertImageType(hObject1, out HObject imageConvert, "byte");
                HOperatorSet.GenImageProto(imageConvert, out HObject ImageClear, 255);
                HOperatorSet.TransToRgb(imageConvert, ImageClear, ImageClear, out HObject imageRed, out HObject ImageGreen, out HObject ImageBlue, "hsv");
                HOperatorSet.Compose3(imageRed, ImageGreen, ImageBlue, out HObject multichanneIImage);
                return multichanneIImage;
            }
            catch (Exception ex)
            {
            }
            return Image3D;
        }


        /// <summary>
        /// 获取3D贴图模型
        /// </summary>
        /// <returns></returns>
        public HTuple Get3DAnd2DModel(bool isTriangulate = false)
        {


            if (Model3D != null)
            {
                try
                {
                    if (isTriangulate)
                    {
                        HTuple GenParams = new HTuple("xyz_mapping_max_view_angle", "xyz_mapping_max_view_dir_x", "xyz_mapping_max_view_dir_y", "xyz_mapping_max_view_dir_z", "xyz_mapping_output_all_points");
                        HTuple GenValues = new HTuple(new HTuple(90).TupleRad(), 0, 0, 1, "true");
                        HOperatorSet.TriangulateObjectModel3d(Model3D, "xyz_mapping", GenParams, GenValues, out HTuple NoumnhvObjMode3D, out HTuple objmenon);
                        Model3D = NoumnhvObjMode3D;
                    }
                }
                catch (Exception ex)
                {
                }
                if (HWindID.IsObjectValided(Image2D))
                {
                    try
                    {
                        HOperatorSet.MirrorImage(Image2D, out HObject imageMirror, "row");
                        HOperatorSet.Decompose3(imageMirror, out HObject imageR, out HObject imageG, out HObject imageB);
                        imageMirror.Dispose();
                        HOperatorSet.GetObjectModel3dParams(Model3D, "point_coord_x", out HTuple genX);
                        HOperatorSet.GetObjectModel3dParams(Model3D, "point_coord_y", out HTuple genY);
                        HOperatorSet.GetGrayval(imageR, genY, genX, out HTuple grayvalR);
                        HOperatorSet.GetGrayval(imageG, genY, genX, out HTuple grayvalG);
                        HOperatorSet.GetGrayval(imageB, genY, genX, out HTuple grayvalB);
                        HOperatorSet.SetObjectModel3dAttrib(Model3D, "red", new HTuple(), grayvalR, out HTuple objModel3Dout);
                        HOperatorSet.SetObjectModel3dAttrib(objModel3Dout, "green", new HTuple(), grayvalG, out objModel3Dout);
                        HOperatorSet.SetObjectModel3dAttrib(objModel3Dout, "blue", new HTuple(), grayvalB, out objModel3Dout);
                        Model3D = objModel3Dout;
                    }
                    catch (Exception)
                    {
                    }
                }
            }

            return Model3D;
        }



        [JsonIgnore]
        public HTuple PlenaH { get; set; }

        public double SX { get; set; } = 1;

        public double SY { get; set; } = 1;

        public double SZ { get; set; } = 1;

        public double Tz { get; set; } = 1;
        public int RunID { get; set; }

        public int LibraryID { get; set; }

        /// <summary>
        /// 图片文件地址
        /// </summary>
        public string ImagePahts { get; set; }
        [JsonIgnore]
        public double ShowScale
        {
            get { return 1.00 / (SX / Tz); }
        }


        public Dictionary<string, double> RunTimeDic { get; set; } = new Dictionary<string, double>();

        public void Dispose()
        {
            try
            {
                if (Model3D != null && Model3D.TupleIsHandle())
                { HOperatorSet.ClearObjectModel3d(Model3D); Model3D.Dispose(); }

                if (PlenaH != null && PlenaH.TupleIsHandle())
                {
                    PlenaH.Dispose();
                    HOperatorSet.ClearObjectModel3d(PlenaH);
                }
                if (Image2D != null)
                {
                    Image2D.Dispose();
                }
                if (Image3D != null)
                {
                    Image3D.Dispose();
                }

            }
            catch (Exception ex)
            {

            }
        }



    }
}
