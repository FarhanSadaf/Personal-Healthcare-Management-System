using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace PersonalHealthcareManagementSystem.Models.all
{
    public class Test
    {
        [Required(ErrorMessage = "Please select gender")]
        public string Gender { get; set; }
        [Required(ErrorMessage = "Please enter your age")]
        public int Age { get; set; }
        #region Height
        [Required(ErrorMessage = "Please enter height in feet")]
        public int HeightF { get; set; }    //feet
        [Required(ErrorMessage = "Please enter height in inches")]
        public float HeightI { get; set; }    //inches
        #endregion
        [Required(ErrorMessage = "Please enter your weight")]
        public float Weight { get; set; }
        #region Diet
        public string Fruits { get; set; }
        public string Vegetables { get; set; }
        public string Fish { get; set; }
        public string Wholegrain { get; set; }
        public string Fastfood { get; set; }
        public string Sweets { get; set; }
        public string Beverages { get; set; }
        #endregion
        public string Smoking { get; set; }
        public string Alcohol { get; set; }
        public string Fitness { get; set; }
        #region Blood Pressure
        public int Systolic { get; set; }
        public int Diastolic { get; set; }
        #endregion

        private double GetHeightCm()
        {
            return (Convert.ToDouble(HeightF) * 30.48) + (Convert.ToDouble(HeightI) * 2.54);
        }

        #region Scoring systems
        public double CalculateBMI()
        {
            double h_m = GetHeightCm() / 100;
            return Weight / (h_m * h_m);
        }
        public double CalculateBMR()
        {
            double bmr = 0;
            if (Gender != null && Gender.Equals("male"))
            {
                bmr = 10 * Weight + 6.25 * GetHeightCm() - 5 * Age + 5;
            }
            else if (Gender != null && Gender.Equals("female"))
            {
                bmr = 10 * Weight + 6.25 * GetHeightCm() - 5 * Age - 161;
            }
            return bmr;
        }
        public double CalculateFitness()
        {
            double fitness_percent = 0;
            switch (Convert.ToInt32(Fitness))
            {
                case 0:
                    fitness_percent = 0;
                    break;
                case 1:
                    fitness_percent = 0;
                    break;
                case 2:
                    fitness_percent = 5;
                    break;
                case 3:
                    fitness_percent = 35;
                    break;
                case 4:
                    fitness_percent = 55;
                    break;
                case 5:
                    fitness_percent = 85;
                    break;
                case 6:
                    fitness_percent = 100;
                    break;
                case 7:
                    fitness_percent = 100;
                    break;
            }
            return fitness_percent;
        }
        public double CalculateSmoking()
        {
            double smoking_percent = 0;
            switch (Convert.ToInt32(Smoking))
            {
                case 0:
                    smoking_percent = 0;
                    break;
                case 1:
                    smoking_percent = 20;
                    break;
                case 2:
                    smoking_percent = 60;
                    break;
                case 3:
                    smoking_percent = 80;
                    break;
                case 4:
                    smoking_percent = 82;
                    break;
                case 5:
                    smoking_percent = 100;
                    break;
            }
            return smoking_percent;
        }
        public double CalculateAlcohol()
        {
            double alcohol_percent = 0;
            switch (Convert.ToInt32(Alcohol))
            {
                case 0:
                    alcohol_percent = 40;
                    break;
                case 1:
                    alcohol_percent = 60;
                    break;
                case 2:
                    alcohol_percent = 75;
                    break;
                case 3:
                    alcohol_percent = 100;
                    break;
            }
            return alcohol_percent;
        }
        public string CalculateBloodPressure()
        {
            if (Systolic < 120 && Diastolic < 80)
            {
                return "NORMAL";
            }
            else if ((Systolic >= 120 && Systolic <= 129) && Diastolic < 80)
            {
                return "ELEVATED";
            }
            else if ((Systolic >= 130 && Systolic <= 139) || (Diastolic >= 80 && Diastolic <= 89))
            {
                return "HIGH BLOOD PRESSURE (HYPERTENSION) STAGE 1";
            }
            else if (Systolic >= 140 || Diastolic >= 90)
            {
                return "HIGH BLOOD PRESSURE (HYPERTENSION) STAGE 2";
            }
            else if (Systolic >= 180 || Diastolic >= 120)
            {
                return "HYPERTENSIVE CRISIS (consult your doctor immediately)";
            }
            return null;
        }
        public double CalculateBpMAP()
        {
            return ((2 * Diastolic) + Systolic) / 3;
        }
        public double CalculateDiet()
        {
            int total_items = 7;
            double fruit_score = 0, veg_score = 0, fish_score = 0, wg_score = 0, fastfood_score = 0, sweets_score = 0, beverage_score = 0;
            #region Fruit
            if (Fruits == null)
            {
                total_items--;
            }
            else if (Fruits != null && Fruits.Trim().Equals("Often"))
            {
                fruit_score = 10;
            }
            else if (Fruits != null && Fruits.Trim().Equals("Sometimes"))
            {
                fruit_score = 5;
            }
            else if (Fruits != null && Fruits.Trim().Equals("Rarely"))
            {
                fruit_score = 0;
            }
            #endregion
            #region Vegetables
            if (Vegetables == null)
            {
                total_items--;
            }
            else if (Vegetables != null && Vegetables.Trim().Equals("Often"))
            {
                veg_score = 10;
            }
            else if (Vegetables != null && Vegetables.Trim().Equals("Sometimes"))
            {
                veg_score = 5;
            }
            else if (Vegetables != null && Vegetables.Trim().Equals("Rarely"))
            {
                veg_score = 0;
            }
            #endregion
            #region Fish
            if (Fish == null)
            {
                total_items--;
            }
            else if (Fish != null && Fish.Trim().Equals("Often"))
            {
                fish_score = 10;
            }
            else if (Fish != null && Fish.Trim().Equals("Sometimes"))
            {
                fish_score = 5;
            }
            else if (Fish != null && Fish.Trim().Equals("Rarely"))
            {
                fish_score = 0;
            }
            #endregion
            #region Wholegrain
            if (Wholegrain == null)
            {
                total_items--;
            }
            else if (Wholegrain != null && Wholegrain.Trim().Equals("Often"))
            {
                wg_score = 10;
            }
            else if (Wholegrain != null && Wholegrain.Trim().Equals("Sometimes"))
            {
                wg_score = 5;
            }
            else if (Wholegrain != null && Wholegrain.Trim().Equals("Rarely"))
            {
                wg_score = 0;
            }
            #endregion
            #region Fastfood
            if (Fastfood == null)
            {
                total_items--;
            }
            else if (Fastfood != null && Fastfood.Trim().Equals("Often"))
            {
                fastfood_score = 0;
            }
            else if (Fastfood != null && Fastfood.Trim().Equals("Sometimes"))
            {
                fastfood_score = 5;
            }
            else if (Fastfood != null && Fastfood.Trim().Equals("Rarely"))
            {
                fastfood_score = 10;
            }
            #endregion
            #region Sweets
            if (Sweets == null)
            {
                total_items--;
            }
            else if (Sweets != null && Sweets.Trim().Equals("Often"))
            {
                sweets_score = 0;
            }
            else if (Sweets != null && Sweets.Trim().Equals("Sometimes"))
            {
                sweets_score = 5;
            }
            else if (Sweets != null && Sweets.Trim().Equals("Rarely"))
            {
                sweets_score = 10;
            }
            #endregion
            #region Beverages
            if (Beverages == null)
            {
                total_items--;
            }
            else if (Beverages != null && Beverages.Trim().Equals("Often"))
            {
                beverage_score = 0;
            }
            else if (Beverages != null && Beverages.Trim().Equals("Sometimes"))
            {
                beverage_score = 5;
            }
            else if (Beverages != null && Beverages.Trim().Equals("Rarely"))
            {
                beverage_score = 10;
            }
            #endregion

            double total_score = 0, score_percent = 0;
            if (total_items != 0)
            {
                total_score = (fruit_score + veg_score + fish_score + wg_score + fastfood_score + sweets_score + beverage_score) / total_items;
                score_percent = total_score * 10;
            }

            return score_percent;
        }
        #endregion

        #region Score Feedback
        public static string[] GetOverallFeedback(double bmiPercentage, double bmrPercentage, double dietPercentage, double smokingPercentage, double alcoholPercentage, double fitnessPercentage, double bpPercentage)
        {
            string[] feedback = new string[2];
            int totalSectors = 7;
            if (dietPercentage == -1)
            {
                dietPercentage = 0;
                totalSectors--;
            }
            if (smokingPercentage == -1)
            {
                smokingPercentage = 0;
                totalSectors--;
            }
            if (alcoholPercentage == -1)
            {
                alcoholPercentage = 0;
                totalSectors--;
            }
            if (fitnessPercentage == -1)
            {
                fitnessPercentage = 0;
                totalSectors--;
            }
            if (bpPercentage == -1)
            {
                bpPercentage = 0;
                totalSectors--;
            }
            double avg_percentage = (bmiPercentage + bmrPercentage + dietPercentage + smokingPercentage + alcoholPercentage + fitnessPercentage + bpPercentage) / totalSectors;
            feedback[0] = Convert.ToInt32(avg_percentage).ToString();
            if (avg_percentage >= 0 && avg_percentage < 20)
            {
                feedback[1] = "Yor current health state is awful. You must take care of your health. You can take advice from health experts about your current health state.";
            }
            else if (avg_percentage >= 20 && avg_percentage < 40)
            {
                feedback[1] = "Yor current health state is not good. You must take care of your health. You can take advice from health experts about your current health state.";
            }
            else if (avg_percentage >= 40 && avg_percentage < 60)
            {
                feedback[1] = "Yor current health state is pretty average but you can't say good. You must take care of your health.";
            }
            else if (avg_percentage >= 60 && avg_percentage < 80)
            {
                feedback[1] = "Yor current health state is average. Try to improve the sectors where you lack. Remember, health is the key to your happiness.";
            }
            else if (avg_percentage >= 80 && avg_percentage <= 100)
            {
                feedback[1] = "You are in good shape. Try to maintain this state. Good luck!";
            }
            return feedback;
        }
        public static List<string> GetOverallBadSections(double bmiPercentage, double bmrPercentage, double dietPercentage, double smokingPercentage, double alcoholPercentage, double fitnessPercentage, double bpPercentage)
        {
            List<string> sectors = new List<string>();
            if (bmiPercentage < 60)
            {
                sectors.Add("BMI");
            }
            if (bmrPercentage < 60)
            {
                sectors.Add("BMR");
            }
            if (dietPercentage < 60 && dietPercentage != -1)
            {
                sectors.Add("Diet");
            }
            if (smokingPercentage < 60 && smokingPercentage != -1)
            {
                sectors.Add("Smoking");
            }
            if (alcoholPercentage < 60 && alcoholPercentage != -1)
            {
                sectors.Add("Alcohol");
            }
            if (fitnessPercentage < 60 && fitnessPercentage != -1)
            {
                sectors.Add("Fitness");
            }
            if (bpPercentage < 60 && bpPercentage != -1)
            {
                sectors.Add("Blood Pressure");
            }
            return sectors;
        }
        public double GetIdealWeight()
        {
            double weight = 0;
            double inch_over = 0;
            if (HeightF == 5)
            {
                inch_over = HeightI;
            }
            else if (HeightF > 5)
            {
                inch_over = (HeightF - 5) * 12 + HeightI;
            }

            if (Gender != null && Gender.Equals("male"))
            {
                weight = 56.2 + 1.41 * inch_over;
            }
            else if (Gender != null && Gender.Equals("female"))
            {
                weight = 53.1 + 1.36 * inch_over;
            }
            return weight;
        }
        public static string[] GetBMIFeedback(double bmi)
        {
            string[] feedback = new string[3];  // { header, description }
            if (bmi < 16)
            {
                feedback[2] = "20";
                feedback[0] = "Severe Thinness";
                feedback[1] = "Apparently your weight is too low. Eat more and try to improve your weight.";
            }
            else if (bmi >= 16 && bmi <= 17)
            {
                feedback[2] = "40";
                feedback[0] = "Moderate Thinness";
                feedback[1] = "Apparently your weight is low. Eat more and try to improve your weight.";
            }
            else if (bmi >= 17 && bmi <= 18.5)
            {
                feedback[2] = "70";
                feedback[0] = "Mild Thinness";
                feedback[1] = "Though your BMI is moderately normal, you should improve your weight a bit more.";
            }
            else if (bmi >= 18.5 && bmi <= 25)
            {
                feedback[2] = "100";
                feedback[0] = "Normal";
                feedback[1] = "Your BMI score is normal. Try to keep it like this.";
            }
            else if (bmi >= 25 && bmi <= 30)
            {
                feedback[2] = "60";
                feedback[0] = "Overweight";
                feedback[1] = "Though your BMI is moderately normal, you should try to decrese your weight.";
            }
            else if (bmi >= 30 && bmi <= 35)
            {
                feedback[2] = "30";
                feedback[0] = "Obese Class I";
                feedback[1] = "You are overweight. Eat carefully and try to decrese your weight.";
            }
            else if (bmi >= 35 && bmi <= 40)
            {
                feedback[2] = "10";
                feedback[0] = "Obese Class II";
                feedback[1] = "You are overweight. Eat carefully and decrese your weight as soon as possible.";
            }
            else if (bmi > 40)
            {
                feedback[2] = "0";
                feedback[0] = "Obese Class III";
                feedback[1] = "You are too much overweight. Please contact with your doctor as soon as possible.";
            }
            return feedback;
        }
        public static string[] GetBMRFeedback(double bmr)
        {
            string[] feedback = new string[2];
            if (bmr < 2067)
            {
                feedback[1] = "100";
                feedback[0] = "You should do little exercise to keep yourself fit.";
            }
            else if (bmr >= 2067 && bmr <= 2368)
            {
                feedback[1] = "80";
                feedback[0] = "You should exercise 1-3 times/week. 15-30 minutes of elevated heart rate activity.";
            }
            else if (bmr >= 2369 && bmr <= 2523)
            {
                feedback[1] = "60";
                feedback[0] = "You should exercise 4-5 times/week. 15-30 minutes of elevated heart rate activity.";
            }
            else if (bmr >= 2524 && bmr <= 2670)
            {
                feedback[1] = "40";
                feedback[0] = "You should do daily exercise or intense exercise 3-4 times/week. 45-120 minutes of elevated heart rate activity.";
            }
            else if (bmr >= 2671 && bmr <= 2971)
            {
                feedback[1] = "20";
                feedback[0] = "You should do intense exercise 6-7 times/week. 45-120 minutes of elevated heart rate activity.";
            }
            else if (bmr > 2971)
            {
                feedback[1] = "10";
                feedback[0] = "You should do very intense exercise daily, or physical job. 2+ hours of elevated heart rate activity.";
            }
            return feedback;
        }
        public static string GetSmokingFeedback(double percent)
        {
            string feedback = "";
            if (percent >= 0 && percent < 20)
            {
                feedback = "You smoke way too much. You will probably at some point be hit by severe illness due to your smoking.";
            }
            else if (percent >= 20 && percent < 60)
            {
                feedback = "You smoke quite a lot which is very bad for your health. Compared to a non-smoker you can expect a shorter life with more years of disease.";
            }
            else if (percent >= 60 && percent < 80)
            {
                feedback = "Even a few cigarettes a day is a severe threat to your health.";
            }
            else if (percent >= 80 && percent < 100)
            {
                if (percent == 82)
                {
                    feedback = "Even though you are not smoking, then other peoples smoking is damaging you as much as if you were an occasional smoker.";
                }
                else
                {
                    feedback = "You don't smoke, which is one of the healthiest decisions you can make for yourself. Good thing you stopped.";
                }
            }
            else if (percent == 100)
            {
                feedback = "You don't smoke, which is one of the healthiest decisions you can make for yourself.";
            }
            return feedback;
        }
        public static string GetAlcoholFeedback(double percent)
        {
            string feedback = "";
            if (percent >= 0 && percent < 50)
            {
                feedback = "Your alcohol intake is too high. Males should not take more than 21 drinks per week and women not more than 14 drinks.It is particularly unhealthy if you have days where you consume a lot of alcohol at once, but if you do so only rarely, then it's kinda ok.";
            }
            else if (percent >= 50 && percent < 70)
            {
                feedback = "Your alcohol intake is moderate, but still a bit too high in regard to health.";
            }
            else if (percent >= 70 && percent < 99)
            {
                feedback = "Your alcohol intake is low - which is a very healthy thing.It is particularly unhealthy if you have days where you consume a lot of alcohol at once, but you do so only rarely, so it's ok.";
            }
            else if (percent == 100)
            {
                feedback = "Your alcohol intake is low - which is a very healthy thing.";
            }
            return feedback;
        }
        public static string[] GetBpMAPFeedback(double map)
        {
            string[] feedback = new string[2];
            if (map >= 70 && map <= 110)
            {
                feedback[1] = "70";
                feedback[0] = "Your blood pressure is in normal range.";
            }
            else
            {
                feedback[1] = "0";
                feedback[0] = "Your blood pressure isn't normal. You should see a doctor.";
            }
            return feedback;
        }
        public static string GetDietFeedback(double percent)
        {
            string feedback = "";
            if (percent >= 0 && percent < 20)
            {
                feedback = "Your diet habit is bad. You eat too much junk food which is very bad for your health. You must eat healthy for your own health sake.";
            }
            else if (percent >= 20 && percent < 40)
            {
                feedback = "Your diet habit is noot good. You must eat much healthy foods to keep yourself in shape.";
            }
            else if (percent >= 40 && percent < 60)
            {
                feedback = "Your diet habit is moderate. You must eat much healthy foods to keep yourself in shape.";
            }
            else if (percent >= 60 && percent < 80)
            {
                feedback = "Your diet habit is almost good. But it can be better if you choose to eat much healthier.";
            }
            else if (percent >= 80 && percent <= 100)
            {
                feedback = "Your diet habit is perfect. Keep it up like this.";
            }
            return feedback;
        }
        public static string GetFitnessFeedback(double percent)
        {
            string feedback = "";
            if (percent >= 0 && percent < 5)
            {
                feedback = "Very low. Your aerobic fitness is so poor that it is a severe threat to your health. It is important that you implement activities in your daily life where you can feel your breathing is increased. Please note that your aerobic fitness is not as high as it could be partly due to your relatively high body weight.";
            }
            else if (percent >= 5 && percent < 35)
            {
                feedback = "Very low. Your aerobic fitness is so poor that it is a severe threat to your health. It is important that you implement activities in your daily life where you can feel your breathing is increased.";
            }
            else if (percent >= 35 && percent < 55)
            {
                feedback = "Low. Your aerobic fitness is too bad and you would be healthier if you could improve your aerobic capacity. Try to get your heart rate up at least 2-3 times each week. Please note that your aerobic fitness is not as high as it could be partly due to your relatively high body weight.";
            }
            else if (percent >= 55 && percent < 85)
            {
                feedback = "Low. Your aerobic fitness is too bad and you would be healthier if you could improve your aerobic capacity. Try to get your heart rate up at least 2-3 times each week.";
            }
            else if (percent >= 85 && percent < 100)
            {
                feedback = "Average. Your aerobic fitness is ok. Please note that your aerobic fitness is not as high as it could be partly due to your relatively high body weight.";
            }
            else if (percent == 100)
            {
                feedback = "Very high. Your aerobic fitness is extremely good.";
            }
            return feedback;
        }
        #endregion
    }
}