'use client'
import UniversalForm from "@/app/[lng]/UI/forms/abstract/UniversalForm";
import IAddProduct from "@/models/product/IAddProduct";
import * as Yup from "yup";
import FormikInput from "@/app/[lng]/UI/forms/inputs/FormikInput";
import FormikSelect from "@/app/[lng]/UI/forms/inputs/FormikSelect";
import {storeApi} from "@/services/storeService";
import {categoryApi} from "@/services/categoryService";
import label from "@/app/[lng]/UI/Label";
import {productApi} from "@/services/productService";
import {useT} from "next-i18next/client";
import {genderApi} from "@/services/genderService";




const addProductSchema = Yup.object({
    name: Yup.string().required(),
    nameUk: Yup.string().required(),
    description: Yup.string().required(),
    descriptionUk: Yup.string().required(),
    categoryId: Yup.string().required(),
    genderId: Yup.string().required(),
})


const CreateProductForm = ({lng, closeModal} : {lng: string, closeModal: ()=>void}) => {
    const { data, isLoading } = storeApi.useGetStoreQuery();
    const {data: category} = categoryApi.useFetchAllCategoriesQuery(lng);
    const {data: gender} = genderApi.useGetAllGendersQuery(lng);
    const [createProduct] = productApi.useCreateProductMutation();
    const {t} = useT('add_product_form')

    const categories = category?.map(category =>
        ({
            value : category.id,
            label: lng === "en" ? category?.name : category?.nameUk
        })
    )?? [];
    const genders = gender?.map(gender =>
        ({
            value: gender.id,
            label: lng === "en" ? gender?.name : gender?.nameUk
        })
    ) ?? [];
    if (isLoading || !data) {
        return <div>Завантаження...</div>; // або скелетон/спінер
    }

    const initialValues: IAddProduct = {
        name: "",
        nameUk: "",
        description: "",
        descriptionUk: "",
        categoryId: "",
        storeId: data.id,
        genderId: '',
    };
    const onSubmit =  async (values: IAddProduct) => {
        await createProduct(values).unwrap();
        closeModal();
    }
    return (
        <UniversalForm
        initialValues={initialValues}
        validationSchema={addProductSchema}
        title={t('title')}
        subtitle={t('subtitle')}
        submitLabel={t('submit')}
        onSubmit={onSubmit}
        >
            <FormikInput<IAddProduct>
                name="name"
                label={t('name')}
                type="text"
                placeholder={t('name_placeholder')}
            />
            <FormikInput<IAddProduct>
                name="nameUk"
                label={t('name_uk')}
                type="text"
                placeholder={t('name_uk_placeholder')}
            />
            <FormikInput<IAddProduct>
                name="description"
                label={t('description')}
                type="text"
                placeholder={t('description_placeholder')}
            />
            <FormikInput<IAddProduct>
                name="descriptionUk"
                label={t('description_uk')}
                type="text"
                placeholder={t('description_uk_placeholder')}
            />
            <FormikSelect name='categoryId' label="Category" options={categories} placeholder={t('select')} />
            <FormikSelect name='genderId' label="Gender" options={genders} placeholder={t('select')} />

        </UniversalForm>
    );
};

export default CreateProductForm;