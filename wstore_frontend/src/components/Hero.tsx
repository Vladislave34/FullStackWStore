import {useTranslations} from "next-intl";
import ButtonAction from "@/UI/ButtonAction";
import Button from "@/UI/Button";
import CategoryList from "@/components/CategoryList";
import Image from "next/image";
import Carousel from "@/components/Carousel";




const Hero = () => {
    const t = useTranslations("hero");

    return (
        <div
            style={{
                background: "var(--surface)",
                borderBottom: "1px solid var(--border)"
                }}
            className=" w-full ">

            <div className="flex flex-col md:flex-row pt-18">


                <div className="w-full md:flex-1 p-4 flex flex-col gap-6">
                    <p
                        style={{ color: "var(--muted)" }}
                        className="text-xs md:text-xs lg:text-sm font-semibold mt-6"
                    >
                        {t("collection").toUpperCase()}
                    </p>
                    <p
                        style={{ color: "var(--text)" }}
                        className="text-2xl md:text-3xl"
                    >
                        {t("title")}
                    </p>
                    <p
                        style={{ color: "var(--muted)" }}
                        className="text-xs md:text-sm lg:text-base font-semibold w-full md:w-[50%]"
                    >
                        {t("subtitle")}
                    </p>
                    <div className="flex flex-row gap-2">
                        <ButtonAction>
                            {t("btn")}
                        </ButtonAction>
                        <Button>
                            {t("sale")}
                        </Button>
                    </div>
                </div>


                <div className="hidden md:flex md:flex-1 ">
                    <Carousel />


                    {/*<Image src="/_3acab0d4-9150-429e-93df-2f4888a33c42.jpeg" alt="hero" width={750} height={351}/>*/}
                </div>

            </div>

            <div className="w-full md:w-[50%] overflow-x-auto">
                <CategoryList all={t("categories.all")}/>
            </div>


        </div>
    );
};

export default Hero;