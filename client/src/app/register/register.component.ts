import { Component, EventEmitter, OnInit, Output } from '@angular/core';
import { AbstractControl, FormBuilder, FormGroup, ValidatorFn, Validators } from '@angular/forms';
import { Router } from '@angular/router';
import { ToastrService } from 'ngx-toastr';
import { AccountService } from '../_services/account.service';
import {  GENDER_LIST, AFFILIATION_LIST, AFFILIATION_DATA } from '../util/constants';

@Component({
  selector: 'app-register',
  templateUrl: './register.component.html',
  styleUrls: ['./register.component.css']
})
export class RegisterComponent implements OnInit {
  @Output() cancelRegister = new EventEmitter();
  registerForm: FormGroup;
  maxDate: Date;
  validationErrors: string[] = [];
  affiliationList = AFFILIATION_LIST;
  genderList = GENDER_LIST;

  constructor(private accountService: AccountService, private toastr: ToastrService, 
    // orgnizationList!: Orgninzation[] --> See OnInit

    private fb: FormBuilder, private router: Router) { }

  ngOnInit(): void {
    // GET Organizations list
    // this.orgnizationList = this.organizationService.getOrganizations() something like that
    this.initializeForm();
    this.maxDate = new Date();
    this.maxDate.setFullYear(this.maxDate.getFullYear() -18);
  }

  initializeForm() {
    this.registerForm = this.fb.group ({
      username: ['', Validators.required],
      gender: ['', Validators.required],
      firstName: ['', Validators.required],
      lastName: ['', Validators.required],
      knownAs: ['', Validators.required],
      email: ['', Validators.required],
      dateOfBirth: ['', Validators.required],
      city: ['', Validators.required],
      province: ['', Validators.required],
      country: ['', Validators.required],
      occupation: [''],
      skills: [''],
      genres: [''],
      affiliation: [[]],
      password: ['', [Validators.required, Validators.minLength(4), Validators.maxLength(8)]],
      confirmPassword: ['', [Validators.required, this.matchValues('password')]]
    });
    this.registerForm.controls.password.valueChanges.subscribe(() => {
      this.registerForm.controls.confirmPassword.updateValueAndValidity();
    })
  }

  matchValues(matchTo: string): ValidatorFn {
    return (control: AbstractControl) => {
      return control?.value === control?.parent?.controls[matchTo].value 
        ? null : {isMatching: true}
    }
  }

  register() {
    // get OrgID's from from value
    const selectedOrgs = this.registerForm
      .get('affiliation')
      .value.flatMap((i) => i.item_id);
    // get actual Org objects to be passed into request body
    const selectedOrgsFinal = AFFILIATION_DATA.filter( (org, i) => org.id === selectedOrgs[i] );
    // add actual org objects to form value
    this.registerForm.patchValue({
        affiliation: selectedOrgsFinal
    }) 

    console.log(this.registerForm.value)
    this.accountService.register(this.registerForm.value).subscribe(res => {
      this.router.navigateByUrl('/members');
    }, error => {
      console.log(error);
      this.validationErrors = error;
    })
  }

  cancel() {
    this.cancelRegister.emit(false);
  }

}
