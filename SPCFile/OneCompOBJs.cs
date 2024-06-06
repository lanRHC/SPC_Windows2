using HalconDotNet;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vision2.vision;

namespace SPC_Windows.SPCFile
{
    /// <summary>
    /// 元件缺陷集合
    /// </summary>
    public class OneCompOBJs
    {
        public void AddCont(string name, OneRObj oneRObj)
        {
            try
            {
                if (!DicOnes.ContainsKey(name))
                {
                    DicOnes.Add(name, new OneComponent() { ComponentID = name });
                }
                if (DicOnes[name].BankName == null || DicOnes[name].BankName == "")
                {
                    DicOnes[name].BankName = oneRObj.BankName;
                }
                if (oneRObj.Row < 0)
                {
                    if (oneRObj.ROI() != null)
                    {
                        HOperatorSet.SmallestRectangle2(oneRObj.ROI(), out HTuple row, out HTuple col, out HTuple phi, out HTuple length1, out HTuple length2);
                        if (row.Length != 0)
                        {
                            oneRObj.Row = row; oneRObj.Length2 = length2; oneRObj.Length1 = length1; oneRObj.Col = col; oneRObj.Anlge = phi.TupleDeg();
                        }
                    }
                }
                DicOnes[name].Row = oneRObj.Row;
                DicOnes[name].Col = oneRObj.Col;
                DicOnes[name].Add(oneRObj);
                if (DicOnes[name].oneRObjs.Count > 1)
                {
                }
            }
            catch (System.Exception)
            {
            }
        }
        /// <summary>
        /// 设置缺陷集合
        /// </summary>
        /// <param name="oneContOBJs"></param>
        public void SetOneContOBJ(OneCompOBJs oneContOBJs)
        {
            try
            {
                foreach (var item in oneContOBJs.DicOnes)
                {
                    if (DicOnes.ContainsKey(item.Key))
                    {
                        try
                        {
                            for (int i = 0; i < item.Value.oneRObjs.Count; i++)
                            {
                                DicOnes[item.Key].Add(item.Value.oneRObjs[i]);
                            }
                        }
                        catch (System.Exception ex)
                        {
                        }
                    }
                    else
                    {
                        DicOnes.Add(item.Key, item.Value);
                    }

                }
            }
            catch (System.Exception ex)
            {
            }
            //DicOnes = oneContOBJs.DicOnes;
        }

        public void Add(OneComponent oneRObj)
        {
            if (!DicOnes.ContainsKey(oneRObj.ComponentID))
            {
                DicOnes.Add(oneRObj.ComponentID, oneRObj);
            }
            else
            {
                DicOnes[oneRObj.ComponentID].oneRObjs.AddRange(oneRObj.oneRObjs);
                DicOnes[oneRObj.ComponentID].dataMinMax = oneRObj.dataMinMax;
            }
        }
        public void AddDic(OneCompOBJs oneCompOBJs)
        {
            try
            {
                foreach (var item in oneCompOBJs.DicOnes)
                {
                    if (!DicOnes.ContainsKey(item.Key))
                    {
                        DicOnes.Add(item.Key, item.Value);
                    }
                }
            }
            catch (System.Exception)
            {
            }
        }
        public void AddDic(OneCompOBJs oneCompOBJs, int PNumnber)
        {
            try
            {
                foreach (var item in oneCompOBJs.DicOnes)
                {
                    if (!DicOnes.ContainsKey(item.Key + "." + PNumnber))
                    {
                        DicOnes.Add(item.Key + "." + PNumnber, item.Value);
                    }
                }
            }
            catch (System.Exception ex)
            {
            }
        }

        /// <summary>
        ///元件缺陷
        /// </summary>
        public Dictionary<string, OneComponent> DicOnes = new Dictionary<string, OneComponent>();

        public void Dispose()
        {
            try
            {
                if (DicOnes != null)
                {
                    foreach (var item in this.DicOnes.Values)
                    {
                        if (item.DrawOBJ != null)
                        {
                            item.DrawOBJ.Dispose();
                        }
                        if (item.GetImage() != null)
                        {
                            item.GetImage().Dispose();
                        }

                        item.NGROI.Dispose();
                        foreach (var itemrobj in item.oneRObjs)
                        {
                            itemrobj.Dispose();
                        }
                        item.oneRObjs.Clear();
                        item.dataMinMax = null;
                    }
                    DicOnes.Clear();
                }
            }
            catch (System.Exception ex)
            {
            }
        }

        [JsonObject(MemberSerialization.OptOut)]
        /// <summary>
        /// 单个元件,_缺陷集合
        /// </summary>
        public class OneComponent
        {
            [JsonIgnore]
            public Image StaitIamge
            {
                get
                {

                    //if (this.Done)
                    //{
                    //    if (this.aOK)
                    //    {
                    //        return global::Vision2.Properties.Resources.OK;
                    //    }
                    //    else
                    //    {
                    //        return global::Vision2.Properties.Resources.security;
                    //    }
                    //}
                    //else
                    //{
                    //    return global::Vision2.Properties.Resources.next;
                    //}
                    return null;
                }
            }

            public OneComponent()
            {

            }
            public void Dispose()
            {
                try
                {
                    if (imaget != null)
                    {
                        imaget.Dispose();
                        imaget = null;
                    }
                    if (Mode3D != null)
                    {
                        Mode3D.Dispose();
                        Mode3D = null;
                    }
                    if (Mode3D != null)
                    {
                        Mode3D.Dispose();
                        Mode3D = null;
                    }
                }
                catch (System.Exception)
                {
                }
            }

            public string RunVisionName
            {
                get
                {
                    if (oneRObjs.Count != 0)
                    {
                        if (oneRObjs[0].RunName == "")
                        {
                        }
                        return oneRObjs[0].RunName;
                    }
                    return null;
                }
            }
            public void Add(OneRObj oneRObj)
            {
                if (!oneRObjs.Contains(oneRObj))
                {
                    oneRObjs.Add(oneRObj);
                }
            }
            /// <summary>
            /// 库号
            /// </summary>
            public string BankName { get; set; }

            /// <summary>
            /// 
            /// </summary>
            public DataMinMax dataMinMax { get; set; }
            /// <summary>
            /// 缺陷集合
            /// </summary>
            public List<OneRObj> oneRObjs { get; set; } = new List<OneRObj>();
            /// <summary>
            /// 主体坐标Row
            /// </summary>
            public double Row;
            /// <summary>
            /// 主体坐标Col
            /// </summary>
            public double Col;

            [DescriptionAttribute("NGNumber"), Category("基础"), DisplayName("NG点")]
            public int NGNumber
            {
                get
                {
                    int number = 0;
                    for (int i = 0; i < oneRObjs.Count; i++)
                    {
                        if (!oneRObjs[i].aOK) number++;
                    }
                    return number;


                }
            }
            /// <summary>
            /// 
            /// </summary>

            [DescriptionAttribute("aOK"), Category("基础"), DisplayName("结果OK")]
            public bool aOK
            {
                get
                {
                    try
                    {
                        if (oneRObjs == null)
                        {
                            return false;
                        }
                        for (int i = 0; i < oneRObjs.Count; i++)
                        {
                            if (!oneRObjs[i].aOK) return false;
                        }
                    }
                    catch (System.Exception)
                    {
                    }
                    return true;
                }
            }
            public void SetOK(bool value)
            {
                for (int i = 0; i < oneRObjs.Count; i++)
                {
                    oneRObjs[i].aOK = value;
                }
            }
            [DescriptionAttribute("AutoOK"), Category("机检"), DisplayName("机器结果")]
            /// <summary>
            /// 机器判断OK
            /// </summary>
            public bool AutoOK
            {
                get
                {
                    for (int i = 0; i < oneRObjs.Count; i++)
                    {
                        if (oneRObjs[i].CNGText != "" && oneRObjs[i].CNGText != "OK" && oneRObjs[i].CNGText.ToLower() != "pass")
                            return false;
                    }
                    return true;
                }
            }
            /// <summary>
            /// 元件名称
            /// </summary>
            [DescriptionAttribute("ComponentID"), Category("基础"), DisplayName("元件名")]

            public string ComponentID { get; set; } = "";

            public int RunID { get; set; }
            /// <summary>
            /// 
            /// </summary>
            public int Location { get; set; }

            [DescriptionAttribute("Done"), Category("结果"), DisplayName("完成复判")]
            public bool Done
            {
                get
                {
                    if (oneRObjs == null)
                    {
                        return false;
                    }
                    for (int i = 0; i < oneRObjs.Count; i++)
                    {
                        if (!oneRObjs[i].Done)
                            return false;
                    }
                    return true;
                }
            }
            public void SetDone(bool value)
            {
                for (int i = 0; i < oneRObjs.Count; i++)
                {
                    oneRObjs[i].Done = value;
                }
            }

            [DescriptionAttribute("NGText"), Category("机检"), DisplayName("NG缺陷")]
            /// <summary>
            /// NG项
            /// </summary>
            public string NGText
            {
                get
                {
                    string data = "";
                    foreach (var item in oneRObjs)
                    {
                        if (!item.Done)
                        {
                            return item.NGText;
                        }
                        data += item.NGText + ";";
                    }
                    return data.Trim(';');
                }
            }
            /// <summary>
            /// 机器结果
            /// </summary>
            [DescriptionAttribute("CNGText机器检测结果"), Category("机检"), DisplayName("机检结果")]

            public string CNGText
            {
                get
                {
                    string data = "";
                    if (oneRObjs == null)
                    {
                        return data;
                    }
                    foreach (var item in oneRObjs)
                    {
                        if (!item.Done)
                        {
                            return item.CNGText;
                        }
                        data += item.CNGText + ";";
                    }
                    return data.Trim(';');
                }
            }

            public List<string> RestStrings()
            {
                foreach (var item in oneRObjs)
                {
                    for (int i = 0; i < item.RestStrings().Count; i++)
                    {
                        if (!restStrings.Contains(item.RestStrings()[i]))
                        {
                            restStrings.Add(item.RestStrings()[i]);
                        }
                    }
                }
                restStrings = restStrings.Distinct().ToList();
                return restStrings;
            }

            public int RestCode
            {
                get;
                set;
            }


            public string ErrorCode { get; set; } = "";

            /// <summary>
            ///
            /// </summary>
            private List<string> restStrings = new List<string>();
            [DescriptionAttribute("RestText"), Category("结果"), DisplayName("复判后缺陷信息")]
            /// <summary>
            ///复判结果
            /// </summary>
            public string RestText
            {
                get
                {
                    string data = "";
                    foreach (var item in oneRObjs)
                    {
                        if (!item.Done)
                        {
                            return item.RestText;
                        }
                        data += item.RestText + ";";
                    }
                    return data.Trim(';');
                }
            }
            public HObject GetNGobj()
            {
                HObject hObject = new HObject();
                hObject.GenEmptyObj();
                foreach (var item in oneRObjs)
                {
                    if (item.NGROI != null && item.NGROI.IsInitialized())
                    {
                        hObject = hObject.ConcatObj(item.NGROI);
                    }

                }

                return hObject;
            }
            [Browsable(false)]
            public HObject NGROI
            {
                get
                {
                    foreach (var item in oneRObjs)
                    {
                        if (!item.Done)
                        {
                            return item.NGROI;
                        }
                    }
                    HObject hObject = new HObject();
                    hObject.GenEmptyObj();
                    return hObject;
                }
            }

            /// <summary>
            /// 
            /// </summary>
            /// <returns></returns>
            public HObject ROI()
            {
                HObject hObject = new HObject();
                hObject.GenEmptyObj();
                foreach (var item in oneRObjs)
                {
                    if (HWindID.IsObjectValided(item.ROI()))
                    {
                        hObject = hObject.ConcatObj(item.ROI());
                    }

                }
                return hObject;
            }
            /// <summary>
            /// 
            /// </summary>
            /// <param name="hObject"></param>
            /// <returns></returns>
            public HObject GetImage(HObject hObject = null)
            {
                if (hObject != null)
                {
                    imaget = hObject;
                }
                return imaget;
            }
            /// <summary>
            /// 
            /// </summary>
            /// <returns></returns>
            public HTuple Get3DModel(HTuple model3D = null)
            {
                if (model3D != null)
                {
                    Mode3D = model3D;
                }
                else
                {
                    if (Mode3D != null)
                    {
                      
                            if (HWindID.IsObjectValided(imaget))
                            {
                                if (true)
                                {
                                    HOperatorSet.ConvexHullObjectModel3d(Mode3D, out HTuple smoothobje);
                                    //HOperatorSet.SmoothObjectModel3d(Mode3D, "mls", "mls_kNN", 150, out HTuple smoothobje);
                                    Mode3D = smoothobje;
                                    //HOperatorSet.SmoothObjectModel3d(Mode3D, "mls", "mls_force_inwards", "true", out HTuple smoothobje);
                                    HTuple GenParams = new HTuple("xyz_mapping_max_view_angle", "xyz_mapping_max_view_dir_x", "xyz_mapping_max_view_dir_y", "xyz_mapping_max_view_dir_z", "xyz_mapping_output_all_points");
                                    HTuple GenValues = new HTuple(new HTuple(90).TupleRad(), 0, 0, 1, "true");

                                    HOperatorSet.TriangulateObjectModel3d(smoothobje, "xyz_mapping", GenParams, GenValues, out HTuple NoumnhvObjMode3D, out HTuple objmenon);
                                    Mode3D = NoumnhvObjMode3D;
                                }
                                HOperatorSet.MirrorImage(imaget, out HObject imageMirror, "row");
                                HOperatorSet.Decompose3(imageMirror, out HObject imageR, out HObject imageG, out HObject imageB);
                                imageMirror.Dispose();
                                HOperatorSet.GetObjectModel3dParams(Mode3D, "point_coord_x", out HTuple genX);
                                HOperatorSet.GetObjectModel3dParams(Mode3D, "point_coord_y", out HTuple genY);
                                HOperatorSet.GetGrayval(imageR, genY, genX, out HTuple grayvalR);
                                HOperatorSet.GetGrayval(imageG, genY, genX, out HTuple grayvalG);
                                HOperatorSet.GetGrayval(imageB, genY, genX, out HTuple grayvalB);
                                HOperatorSet.SetObjectModel3dAttrib(Mode3D, "red", new HTuple(), grayvalR, out HTuple objModel3Dout);
                                HOperatorSet.SetObjectModel3dAttrib(objModel3Dout, "green", new HTuple(), grayvalG, out objModel3Dout);
                                HOperatorSet.SetObjectModel3dAttrib(objModel3Dout, "blue", new HTuple(), grayvalB, out objModel3Dout);
                                Mode3D = objModel3Dout;
                            }
                        
                    }
                }

                return Mode3D;
            }
            ///// <summary>
            ///// 
            ///// </summary>
            ///// <param name="original_3DData"></param>
            ///// <returns></returns>
            //public Original_3DData GetOriginal_3D(Original_3DData original_3DData = null)
            //{
            //    if (original_3DData == null)
            //    {
            //        original_3DDa = original_3DData;
            //    }
            //    return original_3DDa;
            //}
            //Original_3DData original_3DDa;
            private HTuple Mode3D;
            HObject imaget;
            /// <summary>
            /// 
            /// </summary>
            [Browsable(false)]
            public HObject DrawOBJ { get; set; }

            /// <summary>
            /// 修改单个缺陷项为OK
            /// </summary>
            public void RAddOK(int endt = -1)
            {
                try
                {
                    if (endt == -1)
                    {
                        foreach (var item in oneRObjs)
                        {

                            if (!item.Done)
                            {
                                item.RAddOK();
                                break;
                            }
                        }
                    }
                    else
                    {

                        if (!oneRObjs[endt].Done)
                        {
                            oneRObjs[endt].RAddOK();
                        }
                    }


                }
                catch (System.Exception)
                {
                }

            }

            /// <summary>
            ///修改NG缺陷项
            /// </summary>
            /// <param name="restText"></param>
            public void RAddNG(string restText, int IsEndb = -1)
            {

                for (int i = 0; i < oneRObjs.Count; i++)
                {

                    if (IsEndb != -1)
                    {
                        break;
                    }
                    if (!oneRObjs[i].Done)
                    {
                        oneRObjs[i].RAddNG(restText);

                        break;
                    }
                }
            }

        }
    }
}
