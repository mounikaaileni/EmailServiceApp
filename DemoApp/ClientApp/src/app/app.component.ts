import { Component, OnInit } from '@angular/core';
import { Validators, FormGroup, FormControl, FormBuilder } from '@angular/forms';
import { Observable } from 'rxjs';
import { AppService } from './app.service';

function checkEMail(control: FormControl): { [key: string]: boolean } {

  if (control.value) {
    const txtEmail = control.value;
    const emails = txtEmail.split(';');
    let hasError: boolean = false;
    emails.forEach(email => {
      if (email !== '' && !email.match(/^\w+([\.-]?\w+)*@\w+([\.-]?\w+)*(\.\w{2,3})+$/)) {
        hasError = true;
      }
    });
    if (hasError) {
      return { emailInvalid: false };
    } else {
      return null;
    }
  } else {
    return null;
  }

}

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: [ './app.component.scss' ]
})
export class AppComponent {

  constructor(private formBuilder: FormBuilder, private appService: AppService) {

  }

  public validationsForm: FormGroup;
  public formSubmitted: boolean = false;
  public requestInProcess: boolean = false;
  public result: string = '';
  public version: string = '';


  ngOnInit() {
    // creating form group on initialization
    this.validationsForm = this.formBuilder.group({
      email: new FormControl('', Validators.compose([
        Validators.required,
        checkEMail // custom form validation for multiple emailIds
      ])
      ),
      comments: new FormControl('')
    });
  }

  public resetForm() {
    this.validationsForm.reset();
    this.formSubmitted = false;
    this.result = '';
  }

  public validateAndGenerateMail() {
    if (this.validationsForm.valid) {
      this.requestInProcess = true;

      // fires service call if form is valid
      this.appService.sendMail(this.validationsForm.get("email").value, this.validationsForm.get("comments").value)
        .subscribe(result => {
          this.result = result;
          this.requestInProcess = false;
        });
    }
  }
}
