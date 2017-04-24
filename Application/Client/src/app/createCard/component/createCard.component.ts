import { Component, OnInit, ViewChild } from '@angular/core';
import { FormGroup, FormControl, FormBuilder, Validators } from '@angular/forms';
import { CService } from  '../../_common/services/http.service';
import { MapData } from  '../../mapData/mapData';
import { PopUpMessageComponent } from '../../_common/popup/';
import { ActivatedRoute } from '@angular/router';
import { SettingsService } from '../../_common/services/setting.service';
import { ApiPaths } from  '../../../serverConfig/apiPaths';
import { Http, Headers } from '@angular/http';
import { CookieService } from 'angular2-cookie/core';

const URL = 'http://in-it0289/ListingAPI/api/Listings/PostListing';

@Component({
    selector: 'create-card',
    template: require('../tpls/createCard.html').toString(),
    styles: [require('../styles/createCard.scss').toString()],
    providers: [ApiPaths, SettingsService]
})
export class CreateCardComponent implements OnInit {

    @ViewChild('PopUpMessageComponent') private popUpMessageComponent;
    private myForm: FormGroup;
    private submitted: boolean;
    private selectedCategory: string = 'Automotive';
    private categories;
    private subcategories;
    private uploadedImages = [];
    private uploadedImageData = [];
    private endPoints = [];
    private isActive: string = '';
    private isPublishEnable: boolean = false;
    private updateStatus: boolean = false;
    private isCompleted = [];
    private type: string = '';
    private objDynamicData = {};
    private currentSubCategory: string = '';
    private filters;
    private textBoxes = [];
    private showPopupMessage: boolean = false;
    private showPopupDivMessage: string = '';
    private productId: string;
    private action: string = 'Create';
    private productInfo = {};
    private sessionObj;
    private photos = [];
    private existingImageCount = 0;

    constructor(private httpService: CService,
                private apiPath: ApiPaths,
                private data: MapData,
                private _route: ActivatedRoute,
                private _settingsService: SettingsService,
                private _cookieService: CookieService
                ) {
        let self = this;

        this._route.params.subscribe((params) => {
            self.productId = params['id'];
            if (self.productId !== undefined) {
                self.action = 'Edit';
                self.getCategories(true);
            }
        });
        this.isActive = '';
        this.isCompleted = [];
        this.endPoints.push('SELECT', 'UPLOAD', 'ADD INFO', 'DONE');
        this.type = 'Car-Automotive';
        this.currentSubCategory = 'Car';
        this.filters = [];
        this.sessionObj = this._cookieService.getObject('SESSION_PORTAL');
    }

    public ngOnInit() {
        this.myForm = new FormGroup({
            cardType: new FormControl('', [<any> Validators.required]),
            category: new FormControl('', [<any> Validators.required]),
            subCategory: new FormControl('', [<any> Validators.required]),
            title: new FormControl('', [<any> Validators.required]),
            area: new FormControl('', [<any> Validators.required]),
            city: new FormControl('', [<any> Validators.required]),
            state: new FormControl('', [<any> Validators.required]),
            country: new FormControl('', [<any> Validators.required]),
            shortDesc: new FormControl('', [<any> Validators.required]),
            negotiable: new FormControl('', [<any> Validators.required]),
            price: new FormControl('', [<any> Validators.required]),
            location: new FormControl('', [<any> Validators.required]),
            submittedBy: new FormControl('', []),
            file: new FormControl('', [])
        });
        this.objDynamicData = {};
        this.getCategories();
    }

    private getCategories(editMode = false) {
        let self = this;
        this.httpService.observableGetHttp(this.apiPath.GET_ALL_CATEGORIES, null, false)
        .subscribe((res) => {
           self.categories = res;
           self.subcategories = self.categories[0].SubCategory;
           if (editMode) {
                self.getProductData(self.productId);
            }
        },
        (error) => {
            console.log('error in response');
        },
        () => {
            console.log('Finally');
        });
    }

    private updatePublishStatus(event = null) {
        if (this.action !== 'Edit' || this.updateStatus === true) {
            this.isCompleted.length = 0;
            if ( this.selectedCategory !== '' && this.currentSubCategory !== ''
            && this.checkFormControlStatus(['cardType'])) {
                this.isCompleted.push(this.endPoints[0]);
            }
            if (this.uploadedImages.length > 0) {
                this.isCompleted.push(this.endPoints[1]);
            }
            if (this.checkFormControlStatus(['title', 'shortDesc', 'city', 'price'])) {
                this.isCompleted.push(this.endPoints[2]);
            }
            let status = true;
            if (this.checkFormControlStatus(['cardType', 'title', 'shortDesc', 'city', 'price'])
            && this.selectedCategory !== '' && this.currentSubCategory !== ''
            && (this.uploadedImages.length > 0 || this.photos.length > 0)) {
                switch (this.selectedCategory) {
                    case 'Automotive':
                        if (this.checkFormControlStatus(['YearOfPurchase', 'Brand'])) {
                            if (this.currentSubCategory !== 'Bicycle') {
                                if (this.checkFormControlStatus(['Type', 'KmDriven'])) {
                                    if (this.currentSubCategory === 'Car') {
                                        if (!this.checkFormControlStatus(['FuelType'])) {
                                            status = false;
                                        }
                                    }
                                }else {
                                    status = false;
                                }
                            }
                        }else {
                            status = false;
                        }
                    break;
                    case 'Housing':
                        if (this.checkFormControlStatus(['IdealFor', 'Furnished',
                        'YearOfPurchase'])) {
                            status = this.currentSubCategory !== 'Single Room'
                            ? this.checkFormControlStatus(['Rooms']) : true;
                        }else {
                            status = false;
                        }
                    break;
                    case  'Furniture':
                        status = this.checkFormControlStatus(['DimensionHeight',
                        'DimensionWidth', 'DimensionLength', 'YearOfPurchase']);
                    break;
                    case  'Electronics':
                        status = this.currentSubCategory !== 'Other'
                        ? this.checkFormControlStatus(['Brand', 'YearOfPurchase']) : true;
                    break;
                    case  'Other':
                        status = this.currentSubCategory !== 'Sport Equipment'
                        ? this.checkFormControlStatus(['Type'])
                        : this.checkFormControlStatus(['Brand']);
                    break;
                    default:
                    break;
                }
            }else {
                status = false;
            }
            if (status === true) {
                this.isCompleted.push(this.endPoints[3]);
            }
            this.isPublishEnable = status;
        }
    }

    private checkFormControlStatus(checkData) {
        if (this.myForm) {
            for (let data of checkData) {
                if (this.myForm.value[data]) {
                    let status = this.myForm.value[data] !== '' ? true : false;
                    if (status === false) {
                        return false;
                    }
                }else {
                    return false;
                }
            }
            return true;
        }else {
            return false;
        }
    }

    private getFilters() {
       if (this.subcategories && this.subcategories.length > 0) {
            let url = (this.myForm.get('subCategory').value !== undefined
            && this.myForm.get('subCategory').value !== '')
            ? this.apiPath.FILTERS + this.myForm.get('subCategory').value
            : this.apiPath.FILTERS + this.subcategories[0];
            let self = this;
            this.httpService.observableGetHttp(url, null, false)
            .subscribe((res) => {
                self.filters = res;
                self.loadFilters();
            },
            (error) => {
                console.log('error in response');
            },
            () => {
                console.log('Finally');
            });
        }
    }

    private loadFilters() {
        this.textBoxes = [];
        let filters = this.filters;
        let removeSaleRent = this.filters.Filters.findIndex((x) => x.FilterName === 'Sale/Rent');
        this.filters.Filters.splice( removeSaleRent, 1 )[0];
        let year = this.filters.Filters.findIndex((x) => x.FilterName === 'YearOfPurchase');
        if (year !== -1) {
            this.textBoxes.push(this.filters.Filters.splice( year, 1 )[0]);
        }
        let kmDriven = this.filters.Filters.findIndex((x) => x.FilterName === 'KmDriven');
        if (kmDriven !== -1) {
            this.textBoxes.push(this.filters.Filters.splice( kmDriven, 1 )[0]);
        }
        // for dimensions
        let dimensionLength = this.filters.Filters.findIndex(
            (x) => x.FilterName === 'DimensionLength');
        if (dimensionLength !== -1) {
            this.textBoxes.push(this.filters.Filters.splice( dimensionLength, 1 )[0]);
        }
        let dimensionHeight = this.filters.Filters.findIndex(
            (x) => x.FilterName === 'DimensionHeight');
        if (dimensionHeight !== -1) {
            this.textBoxes.push(this.filters.Filters.splice( dimensionHeight, 1 )[0]);
        }
        let dimensionWidth = this.filters.Filters.findIndex(
            (x) => x.FilterName === 'DimensionWidth');
        if (dimensionWidth !== -1) {
            this.textBoxes.push(this.filters.Filters.splice( dimensionWidth, 1 )[0]);
        }
        let self = this;
        this.filters.Filters.forEach((element) => {
            self.myForm.addControl(element.FilterName, new FormControl('', Validators.required));
        });
        this.textBoxes.forEach((element) => {
            self.myForm.addControl(element.FilterName, new FormControl('', Validators.required));
        });
        self.setFieldValue();
    }

    private reloadSubcategories (category, subCategory = '') {
        this.selectedCategory = category.ListingCategory;
        this.subcategories = category.SubCategory;
        this.currentSubCategory = subCategory !== '' ? subCategory : this.subcategories[0];
        this.type = this.currentSubCategory + '-' + this.selectedCategory;
        this.getFilters();
        this.resetFormValue();
        this.updatePublishStatus();
        if (this.action === 'Edit' && this.productInfo) {
            if (this.productInfo['Listing']) {
                if (this.productInfo['Listing'].IsPublished
                && (this.uploadedImages.length > 0
                || this.photos.length > 0)) {
                    this.isPublishEnable = subCategory !== '' ? true : this.isPublishEnable;
                }
            }
        }
    }

    private fileNameChanged(event) {
        if (this.myForm.get('cardType').value !== ''
        && this.myForm.get('subCategory').value !== ''
        && this.selectedCategory !== '') {
            // this.isCompleted.push(this.endPoints[0]);
        }
        this.isActive = this.endPoints[1];
        if (this.uploadedImages.length < 4) {
            if (event.target.files && event.target.files[0]) {
                this.uploadedImageData.push(event.target.files[0]);
                let reader = new FileReader();
                reader.onload = (e: any) => {
                    this.uploadedImages.push(e.target.result); // Bankim need to check
                };
                reader.readAsDataURL(event.target.files[0]);
                this.updatePublishStatus();
            }
        }
    }

    private resetFormValue() {
        if (this.action !== 'Edit') {
            this.myForm.patchValue({YearOfPurchase: ''});
            this.myForm.patchValue({Brand: ''});
            this.myForm.patchValue({Type: ''});
            this.myForm.patchValue({KmDriven: ''});
            this.myForm.patchValue({FuelType: ''});
            this.myForm.patchValue({IdealFor: ''});
            this.myForm.patchValue({Furnished: ''});
            this.myForm.patchValue({Rooms: ''});
            this.myForm.patchValue({DimensionHeight: ''});
            this.myForm.patchValue({DimensionWidth: ''});
            this.myForm.patchValue({DimensionLength: ''});
        }
    }

    private createCard(action) {
        this.isActive = this.endPoints[3];
        this.myForm.patchValue({category: this.selectedCategory});
        this.myForm.patchValue({submittedBy: this.sessionObj.useremail});
        let cardData = this.data.mapCardData(this.myForm);
        this.showPopupDivMessage = 'saved-listing';
        if (action === 'create') {
            cardData.IsPublished = true;
            this.showPopupDivMessage = 'listing';
        }
        let data = new FormData();
        data.append('listing', JSON.stringify(cardData));
        for (let imageData of this.uploadedImageData) {
            data.append('file', imageData);
        }
        let xhr = new XMLHttpRequest();
        xhr.open('POST', 'http://in-it0289/ListingAPI/api/Listings/PostListing');
        xhr.setRequestHeader('accesstoken', 'c4fd7b85796f4d05b12504fbf1c42a3e');
        xhr.setRequestHeader('useremail', 'avadhut.lakule@globant.com');
        xhr.send(data);
        this.showPopupMessage = true;
        this.submitted = true;
    }

    private isAddInfoCompleted() {
        if (this.myForm.get('title').value !== '' && this.myForm.get('price').value !== ''
        && this.myForm.get('shortDesc').value !== '' && this.myForm.get('location').value !== '') {
            // this.isCompleted.push(this.endPoints[1]);
        }
        this.isActive = this.endPoints[2];
    }

    private subCategoryUpdated() {
        this.type = this.myForm.get('subCategory').value + '-' + this.selectedCategory;
        this.currentSubCategory = this.myForm.get('subCategory').value;
        this.getFilters();
        this.resetFormValue();
        this.updatePublishStatus();
    }

    private getProductData(productId) {
        let productInfoUrl = this._settingsService.getPath('productInfoUrl') + productId;
        console.log('Info URL', productInfoUrl);
        let self = this;
        this.httpService.observableGetHttp(productInfoUrl, null, false)
        .subscribe((res) => {
            self.productInfo = res;
            console.log(res);
            if (self.productInfo['Listing']) {
                self.myForm.patchValue({cardType: self.productInfo['Listing'].ListingType});
                self.myForm.patchValue({title: self.productInfo['Listing'].Title});
                self.myForm.patchValue({location: self.productInfo['Listing'].City});
                self.myForm.patchValue({shortDesc: self.productInfo['Listing'].Details});
                self.myForm.patchValue({price: self.productInfo['Listing'].Price});
                self.myForm.patchValue({area: self.productInfo['Listing'].Address.split('-')[0]});
                self.myForm.patchValue({city: self.productInfo['Listing'].City});
                self.myForm.patchValue({country: self.productInfo['Listing'].Country});
                self.myForm.patchValue({negotiable: self.productInfo['Listing'].Negotiable});
                self.photos = self.productInfo['Listing']['Photos'];
                self.existingImageCount = self.photos.length;
                let categoryIndex;
                if (self.categories) {
                    categoryIndex = self.categories.findIndex((o) => {
                        return o.ListingCategory === self.productInfo['Listing'].ListingCategory;
                    });
                    self.selectedCategory = self.categories[categoryIndex];
              }else {
                console.log('No Category Found !!!');
              }
                self.updateStatus = self.productInfo['Listing'].IsPublished;
                self.currentSubCategory = self.productInfo['Listing'].SubCategory;
                self.reloadSubcategories(self.selectedCategory, self.currentSubCategory);
            }
            },
            (error) => {
                console.log('error in response');
            },
            () => {
                console.log('Finally');
            });
    }

    private setFieldValue() {
        if (this.productInfo) {
            if (this.productInfo['Fields']) {
                for (let product of this.productInfo['Fields']) {
                    this.objDynamicData[product.FieldName] = product.FieldValue;
                }
            }
        }
    }

    private updateCard() {
        let cardData = this.data.mapCardData(this.myForm, this.productInfo['Listing'].IsPublished);
        cardData['_id'] = this.productId;
        cardData['ListingCategory'] = this.selectedCategory;
        cardData['Photos'] = this.photos;
        let url = this.apiPath.UPDATE_CARD;
        let data = new FormData();
        this.showPopupDivMessage = 'card-edit';
        data.append('listing', JSON.stringify(cardData));  // json object
        for (let imageData of this.uploadedImageData) {
            data.append('file', imageData);   // image file object
        }
        let xhr = new XMLHttpRequest();
        xhr.open('PUT', url);
        xhr.setRequestHeader('accesstoken', 'c4fd7b85796f4d05b12504fbf1c42a3e');
        xhr.setRequestHeader('useremail', 'avadhut.lakule@globant.com');
        xhr.send(data);
        this.showPopupMessage = true;
    }

  private removeImage(value) {
      this.uploadedImages.splice(value, 1);
      this.uploadedImageData.splice(value, 1);
      this.updatePublishStatus();
    }

  private removeImagefromPhotos(imgSrc) {
    let deletedImageObj = JSON.stringify(imgSrc);
    let url = this.apiPath.DELETEIMAGE + '?id=' + this.productId;
   // this.photos.delete(1);
    this.existingImageCount = this.existingImageCount > 0 ? this.existingImageCount - 1 : 0 ;
    this.httpService.observablePutHttp(url, deletedImageObj, null, false)
      .subscribe((response) => {
          console.log('deleted the subsscription', response);
          this.getProductData(this.productId);
        },
        (error) => {
          console.log('error in responese');
        },
        () => {
          console.log('finally');
        });
  }
}
