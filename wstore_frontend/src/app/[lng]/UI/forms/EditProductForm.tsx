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
import IProductModel from "@/models/product/IProductModel";
import {genderApi} from "@/services/genderService";




const editProductSchema = Yup.object({
    name: Yup.string().required(),
    nameUk: Yup.string().required(),
    description: Yup.string().required(),
    descriptionUk: Yup.string().required(),

    // categoryId: Yup.string().required(),
})


const EditProductForm = ({lng, closeModal, product} : {lng: string, closeModal: ()=>void, product: IProductModel}) => {
    const { data, isLoading } = storeApi.useGetStoreQuery();
    const {data: category} = categoryApi.useFetchAllCategoriesQuery(lng);
    const {data: gender} = genderApi.useGetAllGendersQuery(lng);
    const [editProduct] = productApi.useEditProductMutation();

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
        name: product.name,
        nameUk: product.nameUk,
        description: product.description,
        descriptionUk: product.descriptionUk,
        categoryId: category?.find(c => c.nameUk === product.category || c.name === product.category)?.id
            ?? category?.[0]?.id
            ?? "",
        genderId: gender?.find(c => c.nameUk === product.gender || c.name === product.gender)?.id
            ?? gender?.[0]?.id
            ?? "",
        storeId: data.id,
    };
    const onSubmit =  async (values: IAddProduct) => {
        await editProduct({id: product.id, model : values}).unwrap();
        closeModal();
    }
    return (
        <UniversalForm
            initialValues={initialValues}
            validationSchema={editProductSchema}
            title="Create Product"
            subtitle="Create Product for ypur store"
            onSubmit={onSubmit}
        >
            <FormikInput<IAddProduct>
                name="name"
                label="Name"
                type="text"
                placeholder="Product Name"
            />
            <FormikInput<IAddProduct>
                name="nameUk"
                label="NameUk"
                type="text"
                placeholder="Product NameUk"
            />
            <FormikInput<IAddProduct>
                name="description"
                label="Description"
                type="text"
                placeholder="Describe Product Description"
            />
            <FormikInput<IAddProduct>
                name="descriptionUk"
                label="DescriptionUk"
                type="text"
                placeholder="Describe Product DescriptionUk"
            />
            <FormikSelect name='categoryId' label="Category" options={categories}  />
            <FormikSelect name='genderId' label="Gender" options={genders}  />
        </UniversalForm>
    );
};

export default EditProductForm;