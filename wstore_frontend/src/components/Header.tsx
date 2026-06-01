
import {useTranslations} from "next-intl";
import Icon from "@/UI/Icon";
import Image from "next/image";
import SearchBar from "@/UI/SearchBar";


import SwitchThemeIcon from "@/UI/SwitchThemeIcon";

import SwitchLocaleIcon from "@/UI/SwitchLocaleIcon";
// import BurgerMenu from "@/UI/BurgerMenu";
import MobileMenu from "@/components/MobileMenu";
import GuestMenu from "@/UI/GuestMenu";
import {TranslationRecord} from "@/types/translations";
import {useAppSelector} from "@/hooks/redux";
import AuthorizeMenu from "@/UI/AuthorizeMenu";
import Menu from "@/UI/Menu";
import {FaCartShopping} from "react-icons/fa6";





const Header = () => {

    const t = useTranslations('nav');
    const t2 = useTranslations('guestmenu');
    const t3 = useTranslations('authorizemenu');
    const guestMenu: TranslationRecord<'guestmenu'> = {
        login: t2('login'),
        register: t2('register'),
    };
    const authorizeMenu: TranslationRecord<'authorizemenu'> = {
        myorders: t3('myorders'),
        profile: t3('profile'),
        logout: t3('logout'),
    };

    // const {isMobile, isDesktop} = useDevice();


    return (

        <nav
            style={{
                background: "var(--surface)",
                borderBottom: "1px solid var(--border)",
            }}
            className="w-full  p-4 flex justify-between items-center h-[75px] fixed top-0 z-50">
            {/*Logo */}
            <div
                style={{color: "var(--text)"}}
                className=" text-xl font-bold hover:cursor-pointer ">
                W
                <span
                    style={{color: "var(--muted)"}}>
                    Store</span>
            </div>


            <div
                style={{color: "var(--muted)"}}
                className="hidden md:flex items-center gap-4 text-base lg:gap-6">
                {["women", "men", "sale"].map((key)=>(
                    <span key={key}
                          className="hover:text-[var(--text)]
                          hover:cursor-pointer transition-colors hover:border-b-2">
                          {t(key as "women" | "men" | "sale")}
                    </span>
                ))}


                {/*<span className="hover:text-[var(--text)]*/}
                {/* hover:cursor-pointer transition-colors hover:border-b-2">*/}
                {/*    {t('men')}*/}
                {/*</span>*/}
                {/*<span className="hover:text-[var(--text)]*/}
                {/* hover:cursor-pointer transition-colors hover:border-b-2">*/}
                {/*    {t('sale')}*/}
                {/*</span>*/}
            </div>

            <div className="hidden md:flex flex-row items-center gap-4 justify-end">

                <SearchBar />
                <SwitchLocaleIcon  />
                <SwitchThemeIcon />
                <Icon height={40} width={40}  >
                    {/*<Image src="/shopping-bags.png" alt="Cart" width={24} height={24} priority />*/}
                    <FaCartShopping size={22} color="var(--accent-mid)"  />

                </Icon>
                <Menu guestMenu={guestMenu} authorizeMenu={authorizeMenu} />


            </div>
            {/*<BurgerMenu  />*/}
            <div className="flex md:hidden items-center gap-3">
                <SearchBar />
                <MobileMenu
                    links={[
                        { key: "women", label: t("women") },
                        { key: "men",   label: t("men") },
                        { key: "sale",  label: t("sale") },
                    ]}
                />
            </div>

        </nav>
    );
};

export default Header;