﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IdlingComplaints.Models.ComplaintForm;
using IdlingComplaints.Tests.ComplaintForm.C10_OverallFunctionality;
using OpenQA.Selenium;
using OpenQA.Selenium.DevTools.V112.Network;
using OpenQA.Selenium.Support.UI;
using SeleniumUtilities.Utils;

namespace IdlingComplaints.Tests.ComplaintForm.P30_EvidenceUpload
{
  // [Parallelizable(ParallelScope.Children)]
  // [FixtureLifeCycle(LifeCycle.InstancePerTestCase)]
    internal class Test10_ComplaintFormFunctionality_UploadFile: FillComplaintForm_Base
    {

        private readonly new int SLEEP_TIMER = 2000;
        private static string IDLING_TRUCK = Directory.GetParent(Directory.GetCurrentDirectory()).Parent.Parent.FullName + "\\Files\\Images\\idling_truck.jpeg";
        private static string IDLING_BUS = Directory.GetParent(Directory.GetCurrentDirectory()).Parent.Parent.FullName + "\\Files\\Images\\idling_bus.jpg";
        private static string IDLING_VAN = Directory.GetParent(Directory.GetCurrentDirectory()).Parent.Parent.FullName + "\\Files\\Images\\idling_van.jpg";
        private static string NOT_SUPPORTED_FILE = Directory.GetParent(Directory.GetCurrentDirectory()).Parent.Parent.FullName + "\\Files\\Images\\not_supported_idling_WEBPfile.webp";
        private static string PDF_FILE = Directory.GetParent(Directory.GetCurrentDirectory()).Parent.Parent.FullName + "\\Files\\Images\\WebDoc.pdf";
        private static string MP4_FILE = Directory.GetParent(Directory.GetCurrentDirectory()).Parent.Parent.FullName + "\\Files\\Images\\MP4_How_To_Get_Rich_Reporting_On_Idling_Vehicles_In_NYC.mp4";
        string[] filePaths = { PDF_FILE, IDLING_BUS, MP4_FILE, NOT_SUPPORTED_FILE };

        [SetUp]
        public void Setup()
        {
            
            base.ComplaintFormModelSetUp(false);
            ClickNo();
            Driver.WaitUntilElementFound(By.CssSelector("input[placeholder='Company Name']"), 15);
            Filled_ComplaintInfo();
            Driver.WaitUntilElementIsNoLongerFound(By.TagName("simple-snack-bar"), 21);

        }
        [TearDown]
        public void Teardown()
        {
            if (SLEEP_TIMER > 0)
                Thread.Sleep(SLEEP_TIMER);
            base.ComplaintFormModelTearDown();
        }

        [Test, Category("Scenario #0: Verify multiple functionalities at once for demo")]
        public void EvidenceUpload_VerifyNotSupportedFile_Upload_Delete_Download_Process()
        {
            /* Upload supported and unsupported files */
            foreach (var file in filePaths)
            {
                EvidenceUpload_UploadControl.SendKeysWithDelay(file, SLEEP_TIMER);
            }
            string commentTest = "Testing the comment box";
            EvidenceUpload_UploadCommentControl.SendKeysWithDelay(commentTest, SLEEP_TIMER);
            EvidenceUpload_ClickFilesUploadConfirm();
            var successfulEvidenceUpload = Driver.WaitUntilElementFound(By.TagName("simple-snack-bar"), 60);
            if (successfulEvidenceUpload != null && successfulEvidenceUpload.Text.Contains("uploaded"))
                Assert.That(successfulEvidenceUpload.Text.Trim(), Contains.Substring("Succesfully uploaded file named: "));
            Thread.Sleep(SLEEP_TIMER);


            /* Delete the first file */
            var deleteControl = Driver.WaitUntilElementFound(By.CssSelector("mat-icon[aria-label='Delete']"), 61); //get delete locator
            EvidenceUpload_ClickDeleteEvidence(); Thread.Sleep(SLEEP_TIMER);
            Driver.WaitUntilElementFound(By.TagName("mat-dialog-container"), 60);
            EvidenceUpload_ConfirmDelete(); 
            Driver.WaitUntilElementIsNoLongerFound(By.TagName("mat-dialog-container"), 62); //
            Thread.Sleep(SLEEP_TIMER);

            /* Download a file */
            var fileList = EvidenceUpload_TableControl.GetDataFromMatTable();
            List<IWebElement> downloadList = fileList.GetSpecificColumnElements(By.CssSelector("mat-icon[aria-label='Download']"));
            for (int i = 0; i < downloadList.Count(); i++)
            {
                fileList = EvidenceUpload_TableControl.GetDataFromMatTable();
                downloadList = fileList.GetSpecificColumnElements(By.CssSelector("mat-icon[aria-label='Download']"));
                downloadList[i].Click();
                Console.WriteLine(i);
                Driver.WaitUntilElementIsNoLongerFound(By.TagName("mat-dialog-container"), 62); //
                Thread.Sleep(SLEEP_TIMER);
            }

            /* Proceed to next page */
            EvidenceUpload_ClickNext(); 
            Driver.WaitUntilElementIsNoLongerFound(By.TagName("mat-dialog-container"), 62); //
            var summons = Driver.WaitUntilElementFound(By.TagName("mat-radio-group"), 62);
            Assert.IsNotNull(summons);
        }


        [Test, Category("Scenario #1: upload one evidence file")]
        public void EvidenceUpload_UploadOneFile()
        {
            string fileName = Path.GetFileName(filePaths[0]);

            EvidenceUpload_UploadControl.SendKeysWithDelay(filePaths[0], SLEEP_TIMER);

            EvidenceUpload_ClickFilesUploadConfirm();

            var successfulEvidenceUpload = Driver.WaitUntilElementFound(By.TagName("simple-snack-bar"), 60);
            Assert.IsNotNull(successfulEvidenceUpload);

            Console.WriteLine(successfulEvidenceUpload.Text);
            if (successfulEvidenceUpload.Text.Contains("uploaded"))
                Assert.That(successfulEvidenceUpload.Text.Trim(), Contains.Substring("Succesfully uploaded file named: " + fileName));

        }


        [Test, Category("Scenario #2: Upload multiple evidence files at once")]
        public void EvidenceUpload_MultipleFileUpload()
        {
          
            foreach (var file in filePaths)
            {
                EvidenceUpload_UploadControl.SendKeysWithDelay(file, SLEEP_TIMER);
            }
              EvidenceUpload_ClickFilesUploadConfirm();

            var successfulEvidenceUpload = Driver.WaitUntilElementFound(By.TagName("simple-snack-bar"), 60);
            
            if (successfulEvidenceUpload!=null && successfulEvidenceUpload.Text.Contains("uploaded")) 
                Assert.That(successfulEvidenceUpload.Text.Trim(), Contains.Substring("Succesfully uploaded file named: "));

        }


        [Test, Category("Scenario #4: After files uploaded, The number of table rows displayed should correspond to the total number of files that have been uploaded.")]
        public void EvidenceUpload_TableApptearanceCheck()
        {
         
            foreach (var file in filePaths)
            {
                EvidenceUpload_UploadControl.SendKeysWithDelay(file, SLEEP_TIMER);
            }
            EvidenceUpload_ClickFilesUploadConfirm();


            var successfulEvidenceUpload = Driver.WaitUntilElementFound(By.TagName("simple-snack-bar"), 61);
            Driver.WaitUntilElementIsNoLongerFound(By.TagName("simple-snack-bar"), 60);


            var rowList = MatTableControl.GetDataFromMatTable(); //Needed to make new version of this because it's a different type of table
            Assert.That(rowList.Count, Is.EqualTo(filePaths.Length-1)); // one file is not supplorted
          

        }


        [Test, Category("Scenario #4: Verify a delete button")]
        public void EvidenceUpload_VerifyOneDeleteButton()
        {
            EvidenceUpload_UploadOneFile();

            var deleteControl = By.CssSelector("mat-icon[aria-label='Delete']"); //get delete locator

            EvidenceUpload_ClickDeleteEvidence();

            Driver.WaitUntilElementFound(By.TagName("mat-dialog-container"), 60);
            EvidenceUpload_ConfirmDelete();
            Driver.WaitUntilElementFound(By.TagName("mat-error"), 61);
            Assert.IsNotNull(EvidenceUpload_UploadErrorControl);
        }


        [Test, Category("Scenario #5: Verify delete buttons by continuously delete files")]
        public void EvidenceUpload_VerifyMultipleDeleteButton()
        {
            EvidenceUpload_MultipleFileUpload();

            Thread.Sleep(SLEEP_TIMER);
            var fileList = EvidenceUpload_TableControl.GetDataFromMatTable();
            Console.WriteLine("Total uploaded files:  " + (fileList.Count-1));

            List<IWebElement> deleteFileList = fileList.GetSpecificColumnElements(By.CssSelector("mat-icon[aria-label='Delete']")); //Gets the Delete button for each row
           
            while(fileList.Count> 1) 
            {
                fileList = EvidenceUpload_TableControl.GetDataFromMatTable();           //Divide into rows from table
                Console.WriteLine(fileList.Count);
                deleteFileList = fileList.GetSpecificColumnElements(By.CssSelector("mat-icon[aria-label='Delete']")); //Gets the Delete button for each row
                deleteFileList[0].Click();
               
                Driver.WaitUntilElementFound(By.TagName("mat-dialog-container"), 61);
                EvidenceUpload_ConfirmDelete();
                
                Driver.WaitUntilElementIsNoLongerFound(By.TagName("mat-dialog-container"), 62);
                Thread.Sleep(SLEEP_TIMER);
            }
        
            Driver.WaitUntilElementFound(By.TagName("mat-error"), 63);
            Assert.IsNotNull(EvidenceUpload_UploadErrorControl);

        }


        [Test, Category("Scenario #6: Verify the comment text box")]
        public void EvidenceUpload_Comment()
        {
            foreach (var file in filePaths)
            {
                EvidenceUpload_UploadControl.SendKeysWithDelay(file, SLEEP_TIMER);
            }
            
            string commentTest = "Testing the comment box";
            EvidenceUpload_UploadCommentControl.SendKeysWithDelay(commentTest, SLEEP_TIMER);
            EvidenceUpload_ClickFilesUploadConfirm();

            Driver.WaitUntilElementFound(By.XPath("//mat-card/mat-card-content/div/mat-table/mat-row[1]/mat-cell[3]"), 61);
            string commentUploadedFiles = Driver.FindElement(By.XPath("//mat-card/mat-card-content/div/mat-table/mat-row[1]/mat-cell[3]")).Text;

            Assert.That(commentTest, Is.EqualTo(commentUploadedFiles));
        }


        [Test, Category("Scenario #7: Verify the Cancel file upload button")]
        public void EvidenceUpload_CancelFileUploadButton()
        {
            foreach (var file in filePaths)
            {
                EvidenceUpload_UploadControl.SendKeysWithDelay(file, SLEEP_TIMER);
            }
            string commentTest = "Testing the comment box";
            EvidenceUpload_UploadCommentControl.SendKeysWithDelay(commentTest, SLEEP_TIMER);
            
            EvidenceUpload_UploadCancelControl.Click();

            Driver.WaitUntilElementFound(By.TagName("mat-header-cell"), 61);
            Assert.IsNotNull(EvidenceUpload_UploadErrorControl);
        }


        [Test, Category("Scenario #8: Verify the download button")]
        public void EvidenceUpload_DownloadFileButton()
        {
            EvidenceUpload_MultipleFileUpload();
            Thread.Sleep(1000);

            var fileList = EvidenceUpload_TableControl.GetDataFromMatTable();
            List<IWebElement> downloadList = fileList.GetSpecificColumnElements(By.CssSelector("mat-icon[aria-label='Download']"));
         
            for(int i=0; i< downloadList.Count(); i++)
            {
                fileList = EvidenceUpload_TableControl.GetDataFromMatTable();
                downloadList = fileList.GetSpecificColumnElements(By.CssSelector("mat-icon[aria-label='Download']"));
                downloadList[i].Click();
                Console.WriteLine(i);
                Thread.Sleep(3000);
            }
        }


        [Test, Category("Scenario #9: Verify the Previous button")]
        public void EvidenceUpload_PreviousButton()
        {
            EvidenceUpload_ClickPrevious();
            var title = Driver.WaitUntilElementFound(By.CssSelector("mat-icon[mattooltipposition='right']"), 65);
            Assert.That(title.Text, Is.EqualTo("help"));

        }


        [Test, Category("Scenario #10: Verify the Next button")]
        public void EvidenceUpload_NextButton()
        {
            EvidenceUpload_MultipleFileUpload();
            //Driver.WaitUntilElementFound(By.CssSelector("button[type='submit']"), 61);'
            Driver.WaitUntilElementIsNoLongerFound(By.TagName("simple-snack-bar"), 60); //wait for the narrow bar to dispare, then click the next page
            EvidenceUpload_ClickNext();
       
            
            var NextPate_summonsAffidavit = Driver.WaitUntilElementFound(By.XPath("//form/div/mat-card/mat-card-header/div/mat-card-title/h4"), 63);
            Assert.IsNotNull(NextPate_summonsAffidavit);
       
        }


        [Test, Category("Scenario #11: Verify the Next button")]
        public void EvidenceUpload_CancelEvidenceFileUploadButton()
        {
            EvidenceUpload_ClickCancel();
            var selectYear = Driver.WaitUntilElementFound(By.TagName("mat-select"), 65);
           
            Assert.That(selectYear.Text, Is.EqualTo("Current Year"));
        }

        [Test, Category("Verify Button")]
        public void EvidenceUpload_MissingUpload_VerifyRefreshButton()
        {
            EvidenceUpload_ClickRefresh();

            var appearOath = Driver.WaitUntilElementFound(By.CssSelector("mat-radio-button[value='753720001']"), 60);
            Assert.IsNull(appearOath, "Flagged inconsistency on purpose.");
        }
    }
}
