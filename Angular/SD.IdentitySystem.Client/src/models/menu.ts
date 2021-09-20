import {ApplicationType, ModelBase} from "sd-infrastructure";
import {InfoSystem} from "./info-system";

/*菜单*/
export interface Menu extends ModelBase {

    /*信息系统编号*/
    systemNo: string;

    /*应用程序类型*/
    applicationType: ApplicationType;

    /*链接地址*/
    url: string;

    /*路径*/
    path: string;

    /*图标*/
    icon: string;

    /*排序*/
    sort: number;

    /*是否是根级节点*/
    isRoot: boolean;

    /*是否是叶子级节点*/
    isLeaf: boolean;

    /*上级菜单Id*/
    parentMenuId: string | null;

    /*导航属性 - 信息系统*/
    infoSystemInfo: InfoSystem | null;
}
