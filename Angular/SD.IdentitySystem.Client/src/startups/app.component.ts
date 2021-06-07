import {Component, OnInit} from '@angular/core';
import {PrimeNGConfig} from 'primeng/api';

/*应用程序根组件*/
@Component({
    selector: 'app-root',
    template: `
        <p-toast position="center"></p-toast>
        <p-confirmDialog [style]="{width: '30vw'}" [baseZIndex]="10000" header="警告" icon="pi pi-exclamation-triangle"
                         rejectButtonStyleClass="p-button-text"></p-confirmDialog>
        <router-outlet></router-outlet>
    `
})
export class AppComponent implements OnInit {

    /*PrimeNG Ripple配置*/
    private primengConfig: PrimeNGConfig

    /**
     * 创建应用程序根组件构造器
     * */
    public constructor(primengConfig: PrimeNGConfig) {
        this.primengConfig = primengConfig;
    }

    /**
     * 初始化组件
     * */
    public ngOnInit(): void {
        this.primengConfig.ripple = true;
    }
}
