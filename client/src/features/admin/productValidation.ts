import * as yup from 'yup';

export const validationSchema = yup.object({
    name: yup.string().required(),
    brand: yup.string().required(),
    type: yup.string().required(),
    price: yup.number().transform((value) => (isNaN(value) || value === null || value === undefined) ? 0 : value)
    .required().moreThan(100),
    quantityInStock: yup.number().transform((value) => (isNaN(value) || value === null || value === undefined) ? 0 : value)
    .required().positive(),
    description: yup.string().required(),
    file: yup.mixed().when('productUrl', {
        is: (value: string) => !value,
        then: (schema) => schema.required('Please provide an image')
      })
})