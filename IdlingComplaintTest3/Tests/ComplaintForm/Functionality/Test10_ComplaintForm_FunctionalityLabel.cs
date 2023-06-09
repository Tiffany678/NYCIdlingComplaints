﻿using OpenQA.Selenium;
using SeleniumUtilities.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IdlingComplaints.Tests.ComplaintForm.Functionality
{
    [Parallelizable(ParallelScope.Children)]
    [FixtureLifeCycle(LifeCycle.InstancePerTestCase)]
    internal class Test10_ComplaintForm_FunctionalityLabel : FillComplaintForm_Base
    {

        [SetUp]
        public void SetUp()
        {
            ComplaintFormModelSetUp(true);
        }

        [TearDown]
        public void TearDown()
        {
            ComplaintFormModelTearDown();
        }

        private static string IDLING_TRUCK = Directory.GetParent(Directory.GetCurrentDirectory()).Parent.Parent.FullName + "\\Files\\Images\\idling_truck.jpeg";
        private static string IDLING_BUS = Directory.GetParent(Directory.GetCurrentDirectory()).Parent.Parent.FullName + "\\Files\\Images\\idling_bus.jpg";
        private static string IDLING_VAN = Directory.GetParent(Directory.GetCurrentDirectory()).Parent.Parent.FullName + "\\Files\\Images\\idling_van.jpg";
        private static string NOT_SUPPORTED_FILE = Directory.GetParent(Directory.GetCurrentDirectory()).Parent.Parent.FullName + "\\Files\\Images\\not_supported_idling_WEBPfile.webp";
        private static string PDF_FILE = Directory.GetParent(Directory.GetCurrentDirectory()).Parent.Parent.FullName + "\\Files\\Images\\WebDoc.pdf";
        private static string MP4_FILE = Directory.GetParent(Directory.GetCurrentDirectory()).Parent.Parent.FullName + "\\Files\\Images\\MP4_How_To_Get_Rich_Reporting_On_Idling_Vehicles_In_NYC.mp4";


        [Test]
        [Category("Correct Label Displayed")]
        public void SuccessfulSubmissionMessage()
        {
            base.Filled_ComplaintInfo();
            base.Filled_EvidenceUpload();
            base.Filled_AppearOATH();

            string successMessage = Driver.WaitUntilElementFound(By.TagName("simple-snack-bar"),60).FindElement(By.TagName("span")).Text;

            Assert.That(successMessage, Is.EqualTo("Complaint has been submitted successfully."), "Flagged inconsistency on purpose.");
        }

        [Test]
        [Category("Correct Label Displayed")]
        public void SuccessfulSaveMessage()
        {
            base.Filled_ComplaintInfo();

            var successfulSave = Driver.WaitUntilElementFound(By.TagName("simple-snack-bar"), 20).FindElement(By.TagName("span"));
            Assert.IsNotNull(successfulSave);
            Assert.That(successfulSave.Text.Trim(), Is.EqualTo("This form has been saved successfully."), "Flagged inconsistency on purpose.");
        }


        [Test]
        [Category("Correct Label Displayed")]
        public void SuccessfulUploadEvidenceMessage()
        {
            base.Filled_ComplaintInfo();

            var successfulSave = Driver.WaitUntilElementFound(By.TagName("simple-snack-bar"), 20).FindElement(By.TagName("span"));
            Assert.IsNotNull(successfulSave);
            if (!successfulSave.Text.Contains("saved success")) Assert.That(successfulSave.Text.Trim(), Is.EqualTo("This form has been saved successfully."), "Flagged inconsistency on purpose.");
            Driver.WaitUntilElementIsNoLongerFound(By.TagName("simple-snack-bar"), 20); //message says form is saved

            EvidenceUpload_UploadInput = FILE_IMAGE_PATH;
            string fileName = Path.GetFileName(FILE_IMAGE_PATH);
            EvidenceUpload_ClickFilesUploadConfirm();
            //Thread.Sleep(SLEEPTIMER);

            var successfulEvidenceUpload = Driver.WaitUntilElementFound(By.TagName("simple-snack-bar"), 20).FindElement(By.TagName("span")); // message says evidence have successfully uploaded
            Assert.IsNotNull(successfulEvidenceUpload);
            Assert.That(successfulEvidenceUpload.Text.Trim(), Is.EqualTo("Successfully uploaded file named: " + fileName + "."), "Flagged inconsistency on purpose.");
            
        }

        [Test]
        [Category("Correct Label Displayed")]
        public void SuccessfulUploadDocumentMessage()
        {
            base.Filled_ComplaintInfo();
            base.Filled_EvidenceUpload();

            AppearOATH_ClickNo();

            AppearOATH_UploadFormInput = FILE_IMAGE_PATH;
            string fileName = Path.GetFileName(FILE_IMAGE_PATH);
            AppearOATH_ClickConfirmUpload();


            var successfulDocumentUpload = Driver.WaitUntilElementFound(By.TagName("simple-snack-bar"), 20).FindElement(By.TagName("span")); // message says evidence have successfully uploaded
            Assert.IsNotNull(successfulDocumentUpload);
            Assert.That(successfulDocumentUpload.Text.Trim(), Is.EqualTo("Successfully uploaded file named: " + fileName + "."), "Flagged inconsistency on purpose.");

        }

        [Test]
        [Category("Correct Label Displayed")]
        public void DuplicateSubmissionMessage()
        {
            /*QUALIFYING CRITERIA*/
            QualifyingCriteria();

            /*PERSON OR COMPANY ASSOCIATED TO COMPLAINT*/
            ScrollToZipCode();

            Fill_Associated(false, false, SLEEPTIMER);

            Occurrence_ValidDate();

            Fill_OccurrenceAddress(2, 3, false, SLEEPTIMER);

            Occurrence_SelectVehicleType(2);
            Occurrence_LicensePlateControl.SendKeysWithDelay("DEP1234", SLEEPTIMER);
            Occurrence_SelectLicenseState(1);
            Occurrence_PastOffenseControl.SendKeysWithDelay("Test", SLEEPTIMER);
            Occurrence_SecondPastOffenseControl.SendKeysWithDelay("Test", SLEEPTIMER);

            Fill_InFrontOfSchool(false, SLEEPTIMER);

            Describe_ContentControl.SendKeysWithDelay("Test", SLEEPTIMER);

            ClickWitnessCheckbox();
            ClickSubmitNoCorrectionCheckbox();
            ComplaintInfo_ClickNext();

            Driver.WaitUntilElementIsNoLongerFound(By.TagName("mat-spinner"), 60); // loads to next page 

            base.Filled_EvidenceUpload();
            base.Filled_AppearOATH();

            string duplicateMessage = Driver.WaitUntilElementFound(By.TagName("simple-snack-bar"), 60).FindElement(By.TagName("span")).Text;
            Console.WriteLine(duplicateMessage);
            Assert.True(duplicateMessage.Trim().Contains("This idling complaint has been submitted before: ")
                , "Flagged inconsistency on purpose.");
        }

        [Test]
        [Category("Correct Label Displayed")]
        public void EvidenceUpload_FailedUploadFile_NotSupportedFileType()
        {

            // RegistrationUtilities.UploadFiles(EvidenceUpload_UploadControl, EvidenceUpload_UploadConfirmControl, filePaths);
            Filled_ComplaintInfo();
            string[] filePaths = { NOT_SUPPORTED_FILE, IDLING_TRUCK, IDLING_BUS };
            EvidenceUpload_UploadControl.SendKeysWithDelay(filePaths[0], SLEEPTIMER);


            var failedEvidenceUpload = Driver.WaitUntilElementFound(By.TagName("simple-snack-bar"), 20).FindElement(By.TagName("span"));
            Assert.IsNotNull(failedEvidenceUpload);

            if (failedEvidenceUpload.Text.Contains("Please try a different file type."))
                Assert.That(failedEvidenceUpload.Text.Trim(), Contains.Substring("Please try a different file type. Only the following are allowed: Images, Documents, PDFs, and Videos."));

        }
    }
}
