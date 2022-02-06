import { HttpClient } from '@angular/common/http';
import { Component, OnInit } from '@angular/core';
import { FormControl, FormGroup, Validators } from '@angular/forms';
import { ToastrService } from 'ngx-toastr';
import { Applicant } from '../models/applicant.model';
import { ApplicantService } from '../services/application.service';


@Component({
  selector: 'app-application-input',
  templateUrl: './application-input.component.html',
  styleUrls: ['./application-input.component.css']
})
export class ApplicationInputComponent implements OnInit {

  educationLevels = [
    {display:'Elementary Education', value:'ElementaryEducation'},
    {display:'Secondary Education', value:'SecondaryEducation'},
    {display:'Basic Applied Studies', value:'BasicAppliedStudies'},
    {display:'Master Applied Studies', value:'MasterAppliedStudies'},
    {display:'Doctoral Studies', value:'DoctoralStudies'},

   ];

  submitted = false;
  allLocations: Location[] = [];

  isNew:boolean = true;


  applicant: Applicant = new Applicant();

  newDevice:Applicant = new Applicant();
  newApplicantForm = new FormGroup({
      nameControl : new FormControl('', Validators.required),
      surnameControl : new FormControl('', Validators.required),
      emailControl : new FormControl('', Validators.required),
      educationControl : new FormControl('', Validators.required),
      streetControl : new FormControl('', Validators.required),
      cityControl : new FormControl('', Validators.required),
      countryControl : new FormControl('', Validators.required),
      descriptionControl : new FormControl('', Validators.required),
   });


    

   cvToUpload: File | null = null;
   coverLetterToUpload: File | null = null;
   fileName = '';

   constructor(private http: HttpClient, private applicantService:ApplicantService,  private toastr: ToastrService,) {}

   onFileSelectedCV(event: any) {

       this.cvToUpload = event.target.files[0];

   }

   onFileSelectedCoverLetter(event: any) {

    this.coverLetterToUpload = event.target.files[0];

}

  ngOnInit(): void {
  }

  saveChanges()
  {
    this.submitted = true;

    this.applicant.name = this.newApplicantForm.value.nameControl;
    this.applicant.surname = this.newApplicantForm.value.surnameControl;
    this.applicant.email = this.newApplicantForm.value.emailControl;
    this.applicant.education = this.newApplicantForm.value.educationControl;
    this.applicant.street = this.newApplicantForm.value.streetControl;
    this.applicant.city = this.newApplicantForm.value.cityControl;
    this.applicant.country = this.newApplicantForm.value.countryControl;
    this.applicant.description = this.newApplicantForm.value.descriptionControl;
    

    this.applicantService.createNewApplicant(this.applicant, this.cvToUpload, this.coverLetterToUpload).subscribe(
      data => {      
                    
          this.toastr.success("Successfully submitted!","", {positionClass: 'toast-bottom-left'});
        
        },
        error=>{
          this.toastr.error(error.error);
        
        }
    );

    
  }

//   handleFileInput(files: FileList) {
//     this.fileToUpload = files.item(0);
// }

}
