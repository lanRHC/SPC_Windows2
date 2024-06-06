using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SPC_Windows.SPCFile
{

    /// <summary>
    /// 数据比较类
    /// </summary>
    public class DataMinMax
    {
        public DataMinMax Clone()
        {
            DataMinMax dataMinMax = new DataMinMax();
            for (int i = 0; i < this.Reference_Name.Count; i++)
            {
                dataMinMax.Reference_Name.Add(this.Reference_Name[i]);
            }
            for (int i = 0; i < this.Reference_ValueMax.Count; i++)
            {
                dataMinMax.Reference_ValueMax.Add(this.Reference_ValueMax[i]);
            }
            for (int i = 0; i < this.Reference_ValueMin.Count; i++)
            {
                dataMinMax.Reference_ValueMin.Add(this.Reference_ValueMin[i]);
            }
            for (int i = 0; i < this.valueStrs.Count; i++)
            {
                dataMinMax.valueStrs.Add(this.valueStrs[i]);
            }
            for (int i = 0; i < this.doubleV.Count; i++)
            {
                dataMinMax.doubleV.Add(this.doubleV[i]);
            }
            dataMinMax.ComponentName = this.ComponentName;
            dataMinMax.Describe = this.Describe;
            dataMinMax.Done = this.Done;
            dataMinMax.RunID = this.RunID;
            dataMinMax.OK = this.OK;
            dataMinMax.Reference_ID_Name = this.Reference_ID_Name;

            return dataMinMax;
        }

        public List<string> GetStringData(bool isMax = true)
        {
            List<string> vs = new List<string>();
            if (isMax)
            {
                for (int i2 = 0; i2 < Reference_Name.Count; i2++)
                {
                    vs.Add(Reference_Name[i2] + ":" + Reference_ValueMin[i2].ToString("0.000") + ">" + doubleV[i2].Value.ToString("0.000") + "<" + Reference_ValueMax[i2].ToString("0.000"));
                }
            }
            else
            {
                for (int i2 = 0; i2 < Reference_Name.Count; i2++)
                {
                    vs.Add(Reference_Name[i2] + ":" + doubleV[i2].Value.ToString("0.000"));
                }
            }
            return vs;
        }


        public List<string> GetStrNG()
        {
            List<string> vs = new List<string>();
            try
            {
                for (int i = 0; i < Reference_ValueMin.Count; i++)
                {
                    if (GetRsetNumber(i) != 0)
                    {
                        vs.Add(Reference_Name[i] + ":" + Reference_ValueMin[i] + "<" + doubleV[i].Value.ToString("0.000") + ">" + Reference_ValueMax[i]);
                    }
                }
            }
            catch (Exception)
            {
            }

            return vs;
        }

        public List<string> GetStrTextNG()
        {
            List<string> vs = new List<string>();
            try
            {
                for (int i = 0; i < Reference_ValueMin.Count; i++)
                {
                    if (GetRsetNumber(i) != 0)
                    {
                        vs.Add(GetRsetstr(i));
                    }
                }
            }
            catch (Exception)
            {
            }
            return vs;
        }

        public OneRObj GetOneRObj()
        {
            OneRObj oneRObj = new OneRObj()
            {
                //ComponentID = ComponentName,
            };
            try
            {
                for (int i = 0; i < Reference_Name.Count; i++)
                {
                    oneRObj.RestStrings().Add(Reference_Name[i]);
                }
                if (RunNameOBJ != null && RunNameOBJ.Contains("."))
                {
                    string[] vs = RunNameOBJ.Split('.');
                    if (vs.Length == 2)
                    {
                        //if (RecipeCompiler.GetProductEX().Key_Navigation_Picture.ContainsKey(vs[0]))
                        //{
                        //    if (RecipeCompiler.GetProductEX().Key_Navigation_Picture[vs[0]].GetOriginal_Names().ContainsKey(vs[1]))
                        //    {
                        //        oneRObj.NGROI = RecipeCompiler.GetProductEX().Key_Navigation_Picture[vs[0]].GetOriginal_Names()[vs[1]].ROI.Clone();
                        //        oneRObj.ROI(RecipeCompiler.GetProductEX().Key_Navigation_Picture[vs[0]].GetOriginal_Names()[vs[1]].ROI.Clone());
                        //    }
                        //    else
                        //    {
                        //        AlarmText.AddTextNewLine("不存在导航图坐标:" + vs[1]);
                        //    }
                        //}
                        //else
                        //{
                        //    AlarmText.AddTextNewLine("不存在导航图:" + vs[0]);
                        //}
                    }
                }
                oneRObj.aOK = GetRsetOK();
                if (!oneRObj.aOK)
                {
                    oneRObj.NGText = GetStrTextNG()[0];
                }
                //EntityToEntity(this, oneRObj.dataMinMax);
                oneRObj.dataMinMax = this.Clone();
            }
            catch (Exception)
            {
            }
            //oneRObj.dataMinMax.doubleV[1] = 0;
            return oneRObj;
        }

        public List<OneRObj> GetOneRObjs()
        {
            List<OneRObj> oneRObjs = new List<OneRObj>();
            for (int i = 0; i < Reference_Name.Count; i++)
            {
                if (!GetRestOK(i))
                {
                    OneRObj oneRObj = new OneRObj()
                    {
                        //ComponentID = ComponentName + "." + Reference_Name[i],
                    };
                    oneRObj.AddRestStrings(Reference_Name[i]);
                    try
                    {
                        if (RunNameOBJ != null && RunNameOBJ.Contains("."))
                        {
                            string[] vs = RunNameOBJ.Split('.');
                            if (vs.Length == 2)
                            {
                                //if (RecipeCompiler.GetProductEX().Key_Navigation_Picture.ContainsKey(vs[0]))
                                //{
                                //    if (RecipeCompiler.GetProductEX().Key_Navigation_Picture[vs[0]].GetOriginal_Names().ContainsKey(vs[1]))
                                //    {
                                //        oneRObj.NGROI = RecipeCompiler.GetProductEX().Key_Navigation_Picture[vs[0]].GetOriginal_Names()[vs[1]].ROI.Clone();
                                //        oneRObj.ROI(RecipeCompiler.GetProductEX().Key_Navigation_Picture[vs[0]].GetOriginal_Names()[vs[1]].ROI.Clone());
                                //    }
                                //    else
                                //    {
                                //        AlarmText.AddTextNewLine("不存在导航图坐标:" + vs[1]);
                                //    }
                                //}
                                //else
                                //{
                                //    AlarmText.AddTextNewLine("不存在导航图:" + vs[0]);
                                //}
                            }
                        }
                        oneRObj.aOK = GetRestOK(i);
                        if (!oneRObj.aOK)
                        {
                            if (GetStrTextNG().Count > i)
                            {
                                oneRObj.NGText = Reference_Name[i] + "." + GetStrTextNG()[i];
                            }
                            else
                            {
                                oneRObj.NGText = Reference_Name[i];
                            }
                        }
                        oneRObj.dataMinMax = this.Clone();
                    }
                    catch (Exception ex)
                    {
                    }
                    oneRObjs.Add(oneRObj);
                }
            }
            //oneRObj.dataMinMax.doubleV[1] = 0;
            return oneRObjs;
        }

        public OneCompOBJs.OneComponent GetOneComponent()
        {
            OneCompOBJs.OneComponent oneComponent = new OneCompOBJs.OneComponent();
            try
            {
                oneComponent.ComponentID = ComponentName;
                oneComponent.BankName = "3D";
                if (Describe == null)
                {
                    //foreach (var item in Vision2.vision.Vision.Instance.DicDrawbackNameS)
                    //{
                    //    Describe = item.Key;
                    //    break;
                    //}
                }

                for (int i = 0; i < Reference_Name.Count; i++)
                {
                    OneRObj oneRObj = new OneRObj()
                    {
                        //ComponentID = ComponentName,
                    };
                    if (RunNameOBJ != null && RunNameOBJ.Contains("."))
                    {
                        string[] vs = RunNameOBJ.Split('.');
                        if (vs.Length == 2)
                        {
                            //if (RecipeCompiler.GetProductEX().Key_Navigation_Picture.ContainsKey(vs[0]))
                            //{
                            //    if (RecipeCompiler.GetProductEX().Key_Navigation_Picture[vs[0]].GetOriginal_Names().ContainsKey(vs[1]))
                            //    {
                            //        oneRObj.NGROI = RecipeCompiler.GetProductEX().Key_Navigation_Picture[vs[0]].GetOriginal_Names()[vs[1]].ROI.Clone();
                            //        oneRObj.ROI(RecipeCompiler.GetProductEX().Key_Navigation_Picture[vs[0]].GetOriginal_Names()[vs[1]].ROI.Clone());
                            //    }
                            //    else
                            //    {
                            //        AlarmText.AddTextNewLine("不存在导航图坐标:" + vs[1]);
                            //    }
                            //}
                            //else
                            //{
                            //    AlarmText.AddTextNewLine("不存在导航图:" + vs[0]);
                            //}
                        }
                    }
                    if (GetRsetOK() == false)
                    {
                    }
                    oneRObj.aOK = GetRestOK(i);
                    if (Reference_Name.Count >= i)
                    {
                        oneRObj.BankName = "3D";
                        oneRObj.DefectName = Reference_Name[i];
                        if (!GetRsetOK())
                        {
                            oneRObj.NGText = this.Describe;
                        }
                        //
                    }
                    //if (valueStrs.Count>i)
                    //{
                    //    oneRObj.RestText = valueStrs[i];
                    //}
                    if (!oneRObj.aOK)
                    {
                        oneRObj.RestText = this.Describe;
                        oneRObj.NGText = this.Describe;
                    }
                    else
                    {
                        oneRObj.NGText = "Pass";
                    }
                    //if (!Vision2.vision.Vision.Instance.DicDrawbackNameS.ContainsKey(oneRObj.DefectName))
                    //{
                    //    foreach (var item in Vision2.vision.Vision.Instance.DicDrawbackNameS)
                    //    {
                    //        oneRObj.DefectName = item.Key;
                    //        break;
                    //    }
                    //}
                    oneComponent.Add(oneRObj);
                }
                oneComponent.dataMinMax = this.Clone();

            }
            catch (Exception ex)
            {
            }
            return oneComponent;
        }

        /// <summary>
        /// 将一个实体类复制到另一个实体类
        /// </summary>
        /// <param name="objectsrc">源实体类</param>
        /// <param name="objectdest">复制到的实体类</param>
        public void EntityToEntity(object objectsrc, object objectdest)
        {
            var sourceType = objectsrc.GetType();
            var destType = objectdest.GetType();

            foreach (var source in sourceType.GetProperties())
            {
                if (!source.GetType().IsValueType)
                {
                    foreach (var dest in destType.GetProperties())
                    {
                        if (dest.Name == source.Name)
                        {
                            dest.SetValue(objectdest, source.GetValue(objectsrc));
                            break;
                        }
                    }
                }
                else
                {
                    foreach (var dest in destType.GetProperties())
                    {
                        if (dest.Name == source.Name)
                        {
                            EntityToEntity(source, dest);
                            break;
                            //dest.SetValue(objectdest, source.GetValue(objectsrc));
                        }
                    }
                }
            }
        }

        /// <summary>
        /// 轨迹区域名称
        /// </summary>
        public string RunNameOBJ { get; set; } = "";

        /// <summary>
        /// 元件名称
        /// </summary>
        public string ComponentName { get; set; } = "";

        public bool isColumn { get; set; } = true;
        /// <summary>
        /// 库号和图号ID
        /// </summary>
        public string RunID { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        public string Describe { get; set; }


        public class Reference_Valuves
        {

            /// <summary>
            /// 最小标准值
            /// </summary>
            public double Reference_ValueMin { get; set; } = -99;
            /// <summary>
            /// 最大值
            /// </summary>
            public double Reference_ValueMax { get; set; } = 999;

            public bool GetRest()
            {
                if (doubleV == null)
                {
                    if (double.TryParse(ValueStrs, out double doubleValue))
                    {
                        doubleV = doubleValue;
                    }
                    else
                    {
                        return false;
                    }
                }

                if (Reference_ValueMin > doubleV)
                {
                    return false;
                }
                else if (Reference_ValueMax < doubleV)
                {
                    return false;
                }
                return true;
            }
            public string ComponentName { get; set; }
            /// <summary>
            /// 数据点名称
            /// </summary>
            public string Reference_Name { get; set; } = "";


            public string ValueStrs
            {
                get;
                set;
            } = "";

            public double? doubleV { get; set; }

            public string NGText { get; set; } = "Fail";
            //public HalconDotNet.HObject GetNGROI(string halconName, string crdName)
            //{
            //    return vision.Vision.GetHalconRunName(halconName).Navigation_Picture.GetOriginal_Names()[crdName].ROI.Clone();
            //}

        }

        public List<Reference_Valuves> Reference_Vs { get; set; } = new List<Reference_Valuves>();

        /// <summary>
        /// 最小标准值
        /// </summary>
        public List<double> Reference_ValueMin { get; set; } = new List<double>();

        public List<double> Reference_ValueMax { get; set; } = new List<double>();

        /// <summary>
        /// 数据点名称
        /// </summary>
        public List<string> Reference_Name { get; set; } = new List<string>();

        /// <summary>
        /// 映射ID
        /// </summary>

        public List<string> Reference_ID_Name { get; set; } = new List<string>();

        public List<string> ValueStrs
        {
            get { return valueStrs; }
            set
            {
                valueStrs = value;
                if (doubleV.Count < valueStrs.Count)
                {
                    int DET = valueStrs.Count - doubleV.Count;
                    for (int i = 0; i < DET; i++)
                    {
                        doubleV.Add(0);
                    }
                }
                else if (doubleV.Count > valueStrs.Count)
                {
                    int DET = doubleV.Count - valueStrs.Count;
                    for (int i = 0; i < DET; i++)
                    {
                        if (doubleV.Count > doubleV.Count - 1)
                        {
                            doubleV.RemoveAt(doubleV.Count - 1);
                        }
                    }
                }
                for (int i = 0; i < ValueStrs.Count; i++)
                {
                    if (double.TryParse(ValueStrs[i], out double vael))
                    {
                        doubleV[i] = vael;
                    }
                    else
                    {
                        doubleV[i] = null;
                    }
                }
            }
        }

        private List<string> valueStrs = new List<string>();
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public string GetValuesStr()
        {
            string datas = "";
            for (int i2 = 0; i2 < this.ValueStrs.Count; i2++)
            {
                datas += this.ValueStrs[i2] + ',';
            }
            return datas;
        }
        public List<double?> doubleV { get; set; } = new List<double?>();
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public bool GetRsetOK()
        {
            for (int i = 0; i < Reference_ValueMin.Count; i++)
            {
                if (GetRsetNumber(i) != 0)
                {
                    if (Done)
                    {
                        return OK;
                    }
                    return false;
                }
            }
            return true;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="number"></param>
        /// <returns></returns>
        public bool GetRestOK(int number)
        {
            if (GetRsetNumber(number) != 0)
            {
                return false;
            }
            return true;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public string GetRestText(int id = -1)
        {
            string restText = "";
            if (id < 0)
            {
                for (int i = 0; i < Reference_Name.Count; i++)
                {
                    if (GetRsetNumber(i) == -1)
                    {
                        restText += Reference_Name[i] + ".缺少数据、";
                        return restText;
                    }
                    else if (GetRsetNumber(i) == -2)
                    {
                        restText += Reference_Name[i] + ".Min、";
                    }
                    else if (GetRsetNumber(i) == -3)
                    {
                        restText += Reference_Name[i] + ".Max、";
                    }
                }
            }
            else
            {
                if (GetRsetNumber(id) == -1)
                {
                    restText += Reference_Name[id] + ".缺少数据、";
                    return restText;
                }
                else if (GetRsetNumber(id) == -2)
                {
                    restText += Reference_Name[id] + ".Min、";
                }
                else if (GetRsetNumber(id) == -3)
                {
                    restText += Reference_Name[id] + ".Max、";
                }
            }

            return restText;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public List<Reference_Valuves> ToReference_Valuves()
        {
            List<Reference_Valuves> reference_Valuves = new List<Reference_Valuves>();
            for (int i = 0; i < Reference_Name.Count; i++)
            {

                if (ValueStrs.Count > i)
                {
                    reference_Valuves.Add(new Reference_Valuves() { Reference_Name = Reference_Name[i], Reference_ValueMin = Reference_ValueMin[i], Reference_ValueMax = Reference_ValueMax[i], ValueStrs = ValueStrs[i], ComponentName = ComponentName });

                }
                else
                {
                    reference_Valuves.Add(new Reference_Valuves() { Reference_Name = Reference_Name[i], Reference_ValueMin = Reference_ValueMin[i], Reference_ValueMax = Reference_ValueMax[i], ComponentName = ComponentName });
                }
            }
            return reference_Valuves;
        }
        public void SetResetOK()
        {
            Done = true;
            OK = true;
        }

        public void SetResetNG()
        {
            Done = true;
            OK = false;
        }

        public bool OK { get; set; }

        public bool Done { get; set; } = true;

        public int GetRset()
        {
            if (Reference_Name.Count > ValueStrs.Count)
            {
                return 0;
            }
            for (int i = 0; i < Reference_Name.Count; i++)
            {
                if (GetRsetNumber(i) != 0)
                {
                    return -2;
                }
            }
            return 1;
        }

        /// <summary>
        /// 获取标准值
        /// </summary>
        /// <param name="indxe"></param>
        /// <returns>返回0=OK，-1=空值,返回-2小于下限，返回-3大于上限</returns>
        public int GetRsetNumber(int indxe)
        {
            if (indxe >= doubleV.Count)
            {
                return -1;
            }
            if (doubleV[indxe] == null)
            {
                return -4;
            }
            if (Reference_ValueMin.Count > indxe)
            {
                if (Reference_ValueMin[indxe] > doubleV[indxe])
                {
                    return -2;
                }
                else if (Reference_ValueMax[indxe] < doubleV[indxe])
                {
                    return -3;
                }
            }
            else
            {
                return -5;
            }
            return 0;
        }

        /// <summary>
        /// 获取标准值
        /// </summary>
        /// <param name="indxe"></param>
        /// <returns>返回0=OK，-1=空值,返回-2小于下限，返回-3大于上限</returns>
        public string GetRsetstr(int indxe)
        {
            if (indxe >= doubleV.Count)
            {
                return this.ComponentName + "." + Reference_Name[indxe] + ":缺少数据";
            }
            if (doubleV[indxe] == null)
            {
                return this.ComponentName + "." + Reference_Name[indxe] + ":数据空";
            }
            if (Reference_ValueMin[indxe] > doubleV[indxe])
            {
                return this.ComponentName + "." + Reference_Name[indxe] + ":" + doubleV[indxe] + "超下限";
            }
            else if (Reference_ValueMax[indxe] < doubleV[indxe])
            {
                return this.ComponentName + "." + Reference_Name[indxe] + ":" + doubleV[indxe] + "超上限";
            }
            return "";
        }

        public void AddData(string name, HalconDotNet.HTuple value, double datamin, double datamxa)
        {
            Reference_Name.Add(name);
            Reference_ValueMin.Add(datamin);
            Reference_ValueMax.Add(datamxa);
            for (int i = 0; i < value.Length; i++)
            {
                ValueStrs.Add(value.TupleSelect(i).ToString());
                doubleV.Add(value[i]);
            }
        }

        public void AddData(string name, HalconDotNet.HTuple value)
        {
            Reference_Name.Add(name);
            Reference_ValueMin.Add(value - 999);
            Reference_ValueMax.Add(value + 999);
            for (int i = 0; i < value.Length; i++)
            {
                ValueStrs.Add(value.TupleSelect(i).ToString());
                doubleV.Add(value[i]);
            }
        }

        public void AddData(double value)
        {
            doubleV.Add(value);
            ValueStrs.Add(value.ToString());
        }

        public void Clear()
        {
            ValueStrs.Clear();
            doubleV.Clear();
            if (Reference_Name.Count < Reference_ValueMax.Count)
            {
                Reference_ValueMax.RemoveRange(Reference_Name.Count, Reference_ValueMax.Count - Reference_Name.Count);
            }
            if (Reference_Name.Count < Reference_ValueMin.Count)
            {
                Reference_ValueMin.RemoveRange(Reference_Name.Count, Reference_ValueMin.Count - Reference_Name.Count);
            }
        }
    }
}
