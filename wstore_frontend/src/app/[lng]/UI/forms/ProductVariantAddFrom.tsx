'use client'
import IAddProductVarinat from "@/models/product/variant/IAddProductVarinat";
import * as Yup from "yup";
import UniversalForm from "@/app/[lng]/UI/forms/abstract/UniversalForm";

import FormikFileInput from "@/app/[lng]/UI/forms/inputs/FormikFileInput";
import FormikInput from "@/app/[lng]/UI/forms/inputs/FormikInput";
import FormikSelect from "@/app/[lng]/UI/forms/inputs/FormikSelect";
import {colorApi} from "@/services/colorService";
import {sizeApi} from "@/services/sizeService";

import {productVariantApi} from "@/services/productVariantService";
import {useT} from "next-i18next/client";
import {saleApi} from "@/services/saleService";

const ProductVariantAddFrom = ({lng, closeModal, productId}  : {lng: string, closeModal: ()=>void, productId : string}) => {
    const [addVariant] = productVariantApi.useAddProductVariantMutation();
    const {data: colors} = colorApi.useGetColorsQuery(lng);
    const {t} = useT('create_variant_form');

    const {data: sizes} = sizeApi.useGetSizesQuery();

    const {data : sales } = saleApi.useGetAllSalesQuery();

    const colorOptions = colors?.map(color=>({
        value: color.id,
        label: color.name,
    })) ?? []
    const sizeOptions = sizes?.map(size=>({
        value: size.id,
        label: size.name,
    })) ?? []
    const saleOptions = sales?.map(sale=>({
        value: sale.id,
        label: sale.percent.toString(),
    })).sort((a,b)=> Number(a.label) - Number(b.label)) ?? []


    const initilValues : IAddProductVarinat = {
        productId: productId,
        colorId: '',
        sizeId: '',
        price: 0,
        saleId: '',
        images: [] as File[]
    }
    const schema = Yup.object({
        // colorId: Yup.string().required(),
        // sizeId: Yup.string().required(),
        // price: Yup.number().required(),
        // images: Yup.array()
        //     .of(Yup.mixed<File>().required())
        //     .min(1, "Додайте хоча б одне зображення")
        //     .required()
    })
    const onSubmit = async (values: IAddProductVarinat) => {

        const payload = {
            ...values,
            saleId: values.saleId || null, // порожній рядок -> null для бекенду
        };
        await addVariant(payload).unwrap();
        closeModal();
    }
    type TextInput = Pick<IAddProductVarinat, "price">;
    return (
        <UniversalForm
        initialValues={initilValues}
        validationSchema={schema}
        onSubmit={onSubmit}
        title={t('title')}
        subtitle={t('subtitle')}
        submitLabel={t('submit')}
        >
            <FormikSelect name="sizeId"  label="Size" options={sizeOptions} placeholder={ t('select')} />
            <FormikSelect name="colorId"  label="Color" options={colorOptions} placeholder={ t('select')} />
            <FormikSelect name="saleId"  label="Sale" options={saleOptions} placeholder={ t('select')} />
            <FormikInput<TextInput> name={"price"} label={t("price")} type={"number"} placeholder={"Price"}  />
            <FormikFileInput name={"images"} label={t("images")} multiple={true} />

        </UniversalForm>
    );
};

export default ProductVariantAddFrom;