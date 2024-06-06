using HalconDotNet;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SPC_Windows.SPCFile
{
    [Serializable]
    /// <summary>
    /// 定位信息
    /// </summary>
    public class RModelHomMat
    {
        public RModelHomMat()
        {
            NumberT = Row = Col = Angle = Score = Scale = X = Y = U = 0;
            HomMat = new List<HTuple>();
            //ImageHomMat = new List<HTuple>();
            //this.RowCompensate = new HTuple(0.0);
            //this.ColCompenSate = new HTuple(0.0);
            //this.AngleCompenSate = new HTuple(0.0);
            LocationOK = false;
            ModeXld.GenEmptyObj();
        }

        /// <summary>
        /// 跟随模板
        /// </summary>
        private HObject ModeXld = new HObject();

        public HObject GetModeXld(HObject hObject = null)
        {
            if (hObject != null)
            {
                ModeXld = hObject;
            }
            return ModeXld;
        }

        /// <summary>
        /// 数量
        /// </summary>
        public int NumberT;

        public HTuple Row = new HTuple();

        public HTuple Col = new HTuple();

        /// <summary>
        /// 角度
        /// </summary>
        public HTuple Angle = new HTuple();

        /// <summary>
        /// 角度
        /// </summary>
        public HTuple Phi = new HTuple();

        /// <summary>
        /// 分数
        /// </summary>
        public HTuple Score = new HTuple();

        /// <summary>
        /// 缩放系数
        /// </summary>
        public HTuple Scale = new HTuple();

        /// <summary>
        /// 仿射变换
        /// </summary>
        public List<HTuple> HomMat;

        ///// <summary>
        ///// 仿射变换
        ///// </summary>
        //public List<HTuple> ImageHomMat = new List<HTuple>();
        /// <summary>
        /// 放射模式
        /// </summary>
        public string HomMatMode = "区域放射";

        /// <summary>
        /// 放射XLD
        /// </summary>
        public List<HObject> AffAffineTransContourXld(HObject intHObject)
        {
            List<HObject> listObj = new List<HObject>();
            try
            {
                for (int i = 0; i < this.HomMat.Count; i++)
                {
                    HObject contoursAffineTrans = new HObject();
                    if (HomMatMode == "模板区域")
                    {
                        HOperatorSet.HomMat2dIdentity(out HTuple homMat2D);
                        HOperatorSet.AreaCenterPointsXld(intHObject, out HTuple area, out HTuple row, out HTuple column);
                        HOperatorSet.VectorAngleToRigid(row, column, 0, 0, 0, 0, out homMat2D);
                        HOperatorSet.AffineTransContourXld(intHObject, out contoursAffineTrans, homMat2D);
                    }
                    else
                    {
                        HOperatorSet.AffineTransContourXld(intHObject, out contoursAffineTrans, this.HomMat[i]);
                    }
                    listObj.Add(contoursAffineTrans);
                }
                return listObj;
            }
            catch (Exception)
            {
            }
            return listObj;
        }

        /// <summary>
        /// 定位OK标志
        /// </summary>
        public bool LocationOK;



        /// <summary>
        /// 转换坐标
        /// </summary>
        public HTuple X = new HTuple();

        public HTuple Y = new HTuple();
        public HTuple U = new HTuple();
        public HTuple Z = new HTuple();
        public HTuple V = new HTuple();
        public HTuple W = new HTuple();
    }

}
