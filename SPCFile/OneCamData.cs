using HalconDotNet;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SPC_Windows.SPCFile
{
    public class OneCamData
    {
        public OneCamData()
        {

        }

        /// <summary>
        /// 视觉程序名称
        /// </summary>
        public string RunVisionName;

        /// <summary>
        /// 单面集合是否OK
        /// </summary>
        public bool aOK
        {
            get
            {
                foreach (var item in DicNGObj.DicOnes)
                {
                    if (!item.Value.aOK)
                    {
                        return false;
                    }
                }
                foreach (var item in DicNGObj.DicOnes)
                {
                    if (!item.Value.aOK)
                    {
                        return false;
                    }
                }
                return true;
            }
            //set
            //{
            //    if (!Done)
            //    {
            //        foreach (var item in DicNGObj.DicOnes)
            //        {
            //            item.Value.SetOK( value);
            //        }
            //    }
            //}
        }
        public void SetOK(bool value)
        {
            foreach (var item in DicNGObj.DicOnes)
            {
                item.Value.SetOK(value);
            }
        }

        public void Dispose()
        {
            try
            {
                if (this.ResuOBj() != null)
                {
                    for (int i = 0; i < this.ResuOBj().Count; i++)
                    {
                        if (this.ResuOBj()[i] != null)
                        {
                            this.ResuOBj()[i].Dispose();
                        }
                    }
                }

                this.DicNGObj.Dispose();
                //    this.ImagePlus.Dispose();
                if (resuOBj != null)
                {
                    this.resuOBj.Clear();
                }

                this.resuOBj = null;
            }
            catch (Exception)
            {
            }
        }

        /// <summary>
        /// 单面集合是否完成
        /// </summary>
        public bool Done
        {
            get
            {
                foreach (var item in DicNGObj.DicOnes)
                {
                    if (!item.Value.Done)
                    {
                        return false;
                    }
                }
                return true;
            }
        }

        public void SetDone(bool value)
        {
            foreach (var item in DicNGObj.DicOnes)
            {
                item.Value.SetDone(value);
            }
        }
        /// <summary>
        /// 软件判断是否OK
        /// </summary>
        public bool AutoOK
        {
            get
            {
                foreach (var item in DicNGObj.DicOnes)
                {
                    if (!item.Value.AutoOK)
                    {
                        return false;
                    }
                }
                return true;
            }
        }

        /// <summary>
        /// 误判数量
        /// </summary>
        public int ErrJudNumber
        {
            get
            {
                int number = 0;
                foreach (var item in DicNGObj.DicOnes)
                {
                    if (item.Value.AutoOK != item.Value.aOK)
                    {
                        number++;
                    }
                }
                return number;
            }
        }

        /// <summary>
        /// NG数量
        /// </summary>
        public int NGNumber
        {
            get
            {
                int number = 0;
                foreach (var item in DicNGObj.DicOnes)
                {
                    if (!item.Value.aOK)
                    {
                        number++;
                    }
                }
                return number;
            }
        }

        /// <summary>
        ///已写入
        /// </summary>
        public bool IsClone;

        public HObject GetImagePlus(HObject hObject = null)
        {
            if (hObject != null)
            {
                ImagePlus = hObject;
            }
            return ImagePlus;
        }

        /// <summary>
        /// 整图
        /// </summary>
        public string ImagePaht { get; set; } = "";

        public string ImagePaht2 { get; set; } = "";

        /// <summary>
        /// 整合图片
        /// </summary>
        private HObject ImagePlus = new HObject();

        /// <summary>
        /// 拍照数据集合
        /// </summary>
        private List<OneResultOBj> resuOBj = new List<OneResultOBj>();

        /// <summary>
        /// 拍照数据
        /// </summary>
        /// <returns></returns>
        public List<OneResultOBj> ResuOBj()
        {
            return resuOBj;
        }

        /// <summary>
        /// NG元件
        /// </summary>
        public OneCompOBJs DicNGObj = new OneCompOBJs();

        /// <summary>
        /// 设置缺陷集合
        /// </summary>
        /// <param name="oneContOBJs"></param>
        public void SetOneContOBJ(OneCompOBJs oneContOBJs)
        {
            DicNGObj = oneContOBJs;
        }

        ///// <summary>
        ///// 全部集合
        ///// </summary>
        //public OneCompOBJs AllCompObjs = new OneCompOBJs();
        /// <summary>
        /// 获取全部集合
        /// </summary>
        /// <returns></returns>
        public OneCompOBJs GetAllCompOBJs()
        {
            try
            {
                if (ResuOBj() != null)
                {
                    foreach (var item in ResuOBj())
                    {
                        if (item != null)
                        {
                            foreach (var itemd in item.GetNgOBJS().DicOnes)
                            {
                                if (!DicNGObj.DicOnes.ContainsKey(itemd.Key))
                                {
                                    DicNGObj.DicOnes.Add(itemd.Key, itemd.Value);
                                }
                            }
                        }
                    }
                }

            }
            catch (Exception)
            {
            }
            return DicNGObj;
        }
    }
}
