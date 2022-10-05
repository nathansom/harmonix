import { Component, EventEmitter, OnInit, Output } from '@angular/core';
import { AbstractControl, FormBuilder, FormGroup, ValidatorFn, Validators } from '@angular/forms';
import { Router } from '@angular/router';
import { ToastrService } from 'ngx-toastr';
import { JobsService } from 'src/app/_services/jobs.service';


@Component({
  selector: 'app-job-register',
  templateUrl: './job-register.component.html',
  styles: [
  ]
})
export class JobRegisterComponent implements OnInit {
  @Output() cancelRegister = new EventEmitter();
  registerForm: FormGroup;
  maxDate: Date;
  validationErrors: string[] = [];

  constructor( private jobService: JobsService, private toastr: ToastrService, 
    private fb: FormBuilder, private router: Router) { }

  ngOnInit(): void {
    this.initializeForm();
    this.maxDate = new Date();
    this.maxDate.setFullYear(this.maxDate.getFullYear() -18);
  }

  initializeForm() {
    this.registerForm = this.fb.group ({
      title: ['', Validators.required],
      confirmedOrgId: [''],
      // ! Let's remove logoUrl from this. Let's use OrgPhoto instead (already set by the backend)

      logoUrl: [],

      description: ['', Validators.required],
      salary: [''],
      city: ['', Validators.required],
      provinceOrState: ['', Validators.required],
      country: ['', Validators.required],
      genres: ['', Validators.required],
      jobType: ['', Validators.required],
      skillsRequired: ['', Validators.required],
      applicationUrl: ['', Validators.required],
      deadline: ['', Validators.required]
    });
    // ! The line below causes the errors
    // this.registerForm.controls.password.valueChanges.subscribe(() => {
    //   this.registerForm.controls.confirmPassword.updateValueAndValidity();
    // })
  }

  /*
  strong(): ValidatorFn {
    return (control: AbstractControl) => {
      let hasNumber = /\d/.test(control?.value);
      let hasUpper = /[A-Z]/.test(control.value);
      let hasLower = /[a-z]/.test(control.value);
      // console.log('Num, Upp, Low', hasNumber, hasUpper, hasLower);
      const valid = hasNumber && hasUpper && hasLower;
      if (!valid) {
          // return what´s not valid
          return { strong: true };
      }
      return null;
    }
    
  
    
  }
  */

  matchValues(matchTo: string): ValidatorFn {
    return (control: AbstractControl) => {
      return control?.value === control?.parent?.controls[matchTo].value 
        ? null : {isMatching: true}
    }
  }

  register() {

    console.log(this.registerForm.value);

    this.jobService.registerJob(this.registerForm.value).subscribe(res => {
      this.router.navigateByUrl('/jobs');
    }, error => {
      console.log(error);
      this.validationErrors = error;
    })
  }

  cancel() {
    this.router.navigateByUrl('/jobs');
  }

}
