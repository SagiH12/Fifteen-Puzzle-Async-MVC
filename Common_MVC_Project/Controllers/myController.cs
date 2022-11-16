using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

using Common_MVC_Project.Models;
using System.Web.Script.Serialization;

namespace Common_MVC_Project.Controllers
{
    public class myController : Controller
    {
        static Random myRand = new Random();
        List<myModel> myListModel = new List<myModel>(16);

        public async Task<ActionResult> Index()
        {
            await Task.Run(() => Shuffle());
            return View(myListModel);
        }

        public void Shuffle()
        {
            short i;
            int[] arr = new int[16];
            for (i = 0; i < 15; i++)
                arr[i] = i + 1;
            arr[15] = -1;

            Random myRand = new Random();
            for (i = 14; i > 0; i--)
            {
                int R = myRand.Next(i);
                int temp = arr[i];
                arr[i] = arr[R];
                arr[R] = temp;
            }
            for(i = 0; i < 16; i++)
            {
                myModel tempModel = new myModel();
                if(i != 15)
                {
                    tempModel.Text = arr[i].ToString();
                    tempModel.hexRGB = myRand.Next(50, 226).ToString("x") + myRand.Next(50, 226).ToString("x") + myRand.Next(50, 226).ToString("x");
                }
                myListModel.Add(tempModel);
            }
        }

        [HttpGet]

        public async Task<JsonResult> ShuffleAction()
        {
            await Task.Run(() => Shuffle());
            return Json(myListModel, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]

        public async Task<JsonResult> CurrentStepAction(int indexPushed , string colorPushed , int indexEmpty , string colorEmpty)
        {
            myModel resultModel;
            resultModel = await Task.Run(() => CurrentStep(indexPushed , colorPushed , indexEmpty , colorEmpty));
            return Json(resultModel);
        }

        public myModel CurrentStep(int indexPushed , string colorPushed , int indexEmpty , string colorEmpty)
        {
            int i1 = indexPushed % 4;
            int j1 = indexPushed / 4;

            int i2 = indexEmpty % 4;
            int j2 = indexEmpty / 4;

            myModel resultModel = new myModel();
            if (Math.Abs(i1 - i2) + Math.Abs(j1 - j2) != 1)
                resultModel.Text = "false";
            else
            {
                resultModel.Text = "true";

                string strR_Pushed = colorPushed.Substring(1, 2);
                int R_Pushed = int.Parse(strR_Pushed, System.Globalization.NumberStyles.HexNumber);
                string strG_Pushed = colorPushed.Substring(3, 2);
                int G_Pushed = int.Parse(strG_Pushed, System.Globalization.NumberStyles.HexNumber);
                string strB_Pushed = colorPushed.Substring(5, 2);
                int B_Pushed = int.Parse(strB_Pushed, System.Globalization.NumberStyles.HexNumber);

                string strR_Empty = colorEmpty.Substring(1, 2);
                int R_Empty = int.Parse(strR_Empty, System.Globalization.NumberStyles.HexNumber);
                string strG_Empty = colorEmpty.Substring(3, 2);
                int G_Empty = int.Parse(strG_Empty, System.Globalization.NumberStyles.HexNumber);
                string strB_Empty = colorEmpty.Substring(5, 2);
                int B_Empty = int.Parse(strB_Empty, System.Globalization.NumberStyles.HexNumber);

                int R = (R_Pushed + R_Empty) / 2;
                int G = (G_Pushed + G_Empty) / 2;
                int B = (B_Pushed + B_Empty) / 2;

                resultModel.hexRGB = "#" + R.ToString("x") + G.ToString("x") + B.ToString("x");
            }
            return resultModel;
        }

        [HttpPost]

        public async Task<bool> IsEndAction(string[] arrStr)
        {
            bool result;
            result = await Task.Run(() => IsEnd(arrStr));
            return result;
        }

        public bool IsEnd(string[] arrStr)
        {
            bool result = true;
            for(int i = 0; i < 2; i++)
                if(arrStr[i] != (i + 1).ToString())
                {
                    result = false;
                    break;
                }
                return result;
        }
    }
}
