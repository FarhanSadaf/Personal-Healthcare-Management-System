using PersonalHealthcareManagementSystem.Models;
using PersonalHealthcareManagementSystem.Models.all;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace PersonalHealthcareManagementSystem.Controllers
{
    public class HomeController : Controller
    {
        private PHMSDbContext db = new PHMSDbContext();
        public ActionResult Index()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Index(Test test)
        {
            CheckWhichSetorsNull(test);
            //You can set age to logged users here
            TestResult result = new TestResult();
            result.Weight = test.Weight;
            result.Bmi = test.CalculateBMI();
            result.Bmr = test.CalculateBMR();
            result.DietPercentage = test.CalculateDiet();
            result.SmokingPercentage = test.CalculateSmoking();
            result.AlcoholPercentage = test.CalculateAlcohol();
            result.FitnessPercentage = test.CalculateFitness();
            result.BpMap = test.CalculateBpMAP();
            var bpFeedback = test.CalculateBloodPressure();
            var idealWeight = test.GetIdealWeight();

            if (Session["DietIsNull"] != null && Convert.ToBoolean(Session["DietIsNull"])) result.DietPercentage = -1;
            if (Session["SmokingIsNull"] != null && Convert.ToBoolean(Session["SmokingIsNull"])) result.SmokingPercentage = -1;
            if (Session["AlcoholIsNull"] != null && Convert.ToBoolean(Session["AlcoholIsNull"])) result.AlcoholPercentage = -1;
            if (Session["FitnessIsNull"] != null && Convert.ToBoolean(Session["FitnessIsNull"])) result.FitnessPercentage = -1;
            if (Session["BpIsNull"] != null && Convert.ToBoolean(Session["BpIsNull"])) result.BpMap = -1;

            //Save users TestResult to database
            if (Session["userId"] != null)
            {
                string email = Session["userId"].ToString();
                var v = db.Users.Where(u => u.EmailId == email).FirstOrDefault();
                if (v != null)
                {
                    result.UserId = v.Id;
                    db.TestResults.Add(result);
                    db.SaveChanges();
                }
            }

            //return View(test);
            return RedirectToAction("Result", new
            {
                weight = result.Weight,
                bmi = result.Bmi,
                bmr = result.Bmr,
                dietPercentage = result.DietPercentage,
                smokingPercentage = result.SmokingPercentage,
                alcoholPercentage = result.AlcoholPercentage,
                fitnessPercentage = result.FitnessPercentage,
                bpMap = result.BpMap,
                bpFeedback = bpFeedback,
                idealWeight = idealWeight
            });
        }
        private void CheckWhichSetorsNull(Test test)
        {
            if (test.Fruits == null && test.Vegetables == null && test.Fish == null && test.Wholegrain == null && test.Fastfood == null && test.Sweets == null && test.Beverages == null)
            {
                Session["DietIsNull"] = true;     //Diet
            }
            else
            {
                Session["DietIsNull"] = false;     //Diet
            }

            if (test.Smoking == null)
            {
                Session["SmokingIsNull"] = true;     //Smoking
            }
            else
            {
                Session["SmokingIsNull"] = false;     //Smoking
            }

            if (test.Alcohol == null)
            {
                Session["AlcoholIsNull"] = true;     //Alcohol
            }
            else
            {
                Session["AlcoholIsNull"] = false;     //Alcohol
            }

            if (test.Fitness == null)
            {
                Session["FitnessIsNull"] = true;     //Fitness
            }
            else
            {
                Session["FitnessIsNull"] = false;     //Fitness
            }

            if (test.Systolic == 0 || test.Diastolic == 0)
            {
                Session["BpIsNull"] = true;     //Blood Pressure
            }
            else
            {
                Session["BpIsNull"] = false;     //Blood Pressure
            }

        }

        public ActionResult Result(float weight, double bmi, double bmr, double dietPercentage, double smokingPercentage, double alcoholPercentage, double fitnessPercentage, double bpMap, string bpFeedback, double idealWeight)
        {
            TestResult result = new TestResult(); ;
            result.Weight = weight;
            result.Bmi = bmi;
            result.Bmr = bmr;
            result.DietPercentage = dietPercentage;
            result.SmokingPercentage = smokingPercentage;
            result.AlcoholPercentage = alcoholPercentage;
            result.FitnessPercentage = fitnessPercentage;
            result.BpMap = bpMap;
            double bmiPercentage = Convert.ToDouble(Test.GetBMIFeedback(result.Bmi)[2]);
            double bmrPercentage = Convert.ToDouble(Test.GetBMRFeedback(result.Bmr)[1]);
            double bpPercentage = Convert.ToDouble(Test.GetBpMAPFeedback(result.BpMap)[1]);

            if (Session["BpIsNull"] != null && Convert.ToBoolean(Session["BpIsNull"])) bpPercentage = -1;

            ViewBag.overallFeedback = Test.GetOverallFeedback(bmiPercentage, bmrPercentage, dietPercentage, smokingPercentage, alcoholPercentage, fitnessPercentage, bpPercentage);
            ViewBag.badSectors = Test.GetOverallBadSections(bmiPercentage, bmrPercentage, dietPercentage, smokingPercentage, alcoholPercentage, fitnessPercentage, bpPercentage);
            ViewBag.bmiPercentage = bmiPercentage;
            ViewBag.bmrPercentage = bmrPercentage;
            ViewBag.bpPercentage = bpPercentage;
            ViewBag.BpFeedback = bpFeedback;
            ViewBag.IdealWeight = idealWeight;
            ViewBag.BMIFeedback = Test.GetBMIFeedback(result.Bmi);
            ViewBag.BMRFeedback = Test.GetBMRFeedback(result.Bmr)[0];
            ViewBag.DietFeedback = Test.GetDietFeedback(result.DietPercentage);
            ViewBag.SmokingFeedback = Test.GetSmokingFeedback(result.SmokingPercentage);
            ViewBag.AlcoholFeedback = Test.GetAlcoholFeedback(result.AlcoholPercentage);
            ViewBag.FitnessFeedback = Test.GetFitnessFeedback(result.FitnessPercentage);

            return View(result);
        }

        [Authorize]
        public ActionResult Dashboard()
        {
            List<double> bmiVals = new List<double>();
            List<double> bmrVals = new List<double>();
            List<double> bpVals = new List<double>();
            List<int> dietVals = new List<int>();
            List<int> fitnessVals = new List<int>();
            List<string> bmiDates = new List<string>();
            List<string> dietDates = new List<string>();
            List<string> fitnessDates = new List<string>();
            List<string> bpDates = new List<string>();

            if (Session["userId"] == null)
            {
                return RedirectToAction("Index", "Home");
            }

            string email = Session["userId"].ToString();
            var v = db.Users.Where(u => u.EmailId == email).FirstOrDefault();
            foreach (var result in v.TestResults)
            {
                bmiVals.Add(Math.Round(result.Bmi, 2));
                bmrVals.Add(Math.Round(result.Bmr, 2));
                bmiDates.Add(result.TestDate.ToShortDateString());
                if (result.DietPercentage != -1)
                {
                    dietVals.Add(Convert.ToInt32(result.DietPercentage));
                    dietDates.Add(result.TestDate.ToShortDateString());
                }
                if (result.FitnessPercentage != -1)
                {
                    fitnessVals.Add(Convert.ToInt32(result.FitnessPercentage));
                    fitnessDates.Add(result.TestDate.ToShortDateString());
                }
                if (result.BpMap != -1)
                {
                    bpVals.Add(Math.Round(result.BpMap, 2));
                    bpDates.Add(result.TestDate.ToShortDateString());
                }
            }

            ViewBag.BmiVals = bmiVals;
            ViewBag.BmrVals = bmrVals;
            ViewBag.BpVals = bpVals;
            ViewBag.DietVals = dietVals;
            ViewBag.FitnessVals = fitnessVals;
            ViewBag.BmiDates = bmiDates;
            ViewBag.DietDates = dietDates;
            ViewBag.FitnessDates = fitnessDates;
            ViewBag.BpDates = bpDates;
            return View();
        }

        public ActionResult Specialists()
        {
            var specialists = db.Specialists.ToList();
            return View(specialists);
        }
    }
}