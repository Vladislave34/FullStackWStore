'use client'

import Icon from "@/UI/Icon";
import {setLocale} from "@/utils/setLocale";
import {getLocale} from "@/utils/getLocale";





const SwitchLocaleIcon  = () => {

    const locale = getLocale();
    return (
        <div onClick={()=>setLocale(locale ==="en"? "uk": "en")}>
            <Icon height={40} width={40}>
                {locale}
            </Icon>
        </div>
    );
};

export default SwitchLocaleIcon;