import {Component, OnInit} from '@angular/core';
import {FormBuilder, FormGroup, Validators} from "@angular/forms";
import {NzModalRef} from "ng-zorro-antd/modal";
import {ApplicationType} from "../../../values/enums/application-type";
import {ApplicationTypeDescriptor} from "../../../values/enums/application-type.descriptor";
import {BaseComponent} from "../../../extentions/base.component";
import {InfoSystemService} from "../info-system.service";

/*信息系统创建组件*/
@Component({
    selector: 'app-info-system-add',
    templateUrl: './add.component.html',
    styleUrls: ['./add.component.css']
})
export class AddComponent extends BaseComponent implements OnInit {

    //region # 字段及构造器

    /*对话框引用*/
    private readonly modalRef: NzModalRef;

    /*表单建造者*/
    private readonly formBuilder: FormBuilder;

    /*信息系统服务*/
    private readonly infoSystemService: InfoSystemService;

    /**
     * 创建信息系统创建组件构造器
     * */
    public constructor(modalRef: NzModalRef, formBuilder: FormBuilder, infoSystemService: InfoSystemService) {
        super();
        this.modalRef = modalRef;
        this.formBuilder = formBuilder;
        this.infoSystemService = infoSystemService;
    }

    //endregion

    //region # 属性

    /*信息系统编号*/
    public systemNo: string = "";

    /*信息系统名称*/
    public systemName: string = "";

    /*系统管理员账号*/
    public adminLoginId: string = "";

    /*应用程序类型字典*/
    public applicationTypes: Set<{ key: ApplicationType, value: string }> = ApplicationTypeDescriptor.getEnumMembers();

    /*已选应用程序类型*/
    public selectedApplicationType: ApplicationType | null = null;

    /*表单表单*/
    public formGroup!: FormGroup;

    //endregion

    //region # 方法

    //Initializations

    //region 初始化组件 —— ngOnInit()
    /**
     * 初始化组件
     * */
    public ngOnInit(): void {
        //初始化表单
        this.formGroup = this.formBuilder.group({
            systemNo: [null, [Validators.required]],
            systemName: [null, [Validators.required]],
            adminLoginId: [null, [Validators.required]],
            applicationType: [null, [Validators.required]],
        });
    }
    //endregion


    //Actions

    //region 提交 —— async submit()
    /**
     * 提交
     * */
    public async submit(): Promise<void> {
        for (let index in this.formGroup.controls) {
            this.formGroup.controls[index].markAsDirty();
            this.formGroup.controls[index].updateValueAndValidity();
        }

        if (this.formGroup.valid) {
            this.busy();

            let promise: Promise<void> = this.infoSystemService.createInfoSystem(this.systemNo, this.systemName, this.adminLoginId, this.selectedApplicationType!);
            promise.catch(_ => {
                this.idle();
            });
            await promise;

            this.idle();
            this.modalRef.close(true);
        }
    }
    //endregion

    //region 取消 —— cancel()
    /**
     * 取消
     * */
    public cancel(): void {
        this.modalRef.close(false);
    }
    //endregion

    //endregion
}