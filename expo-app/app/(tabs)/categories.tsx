import {Image, StyleSheet} from 'react-native';

import React, {useEffect, useState} from "react";
import ICategory from "@/app/interfaces/ICategory";
import {FlatList, View, Text} from "react-native";
import axios from "axios";
import {getActionUrl, getImageUrl} from "@/app/apiHelper";

export default function CategoriesScreen() {
    const [categories, setCategories] = useState<ICategory[]>([]);

    useEffect(() => {
        axios.get(getActionUrl("categories", "getall"))
            .then(list => setCategories(list.data))
            .catch(error => console.log(error));
    }, []);

    return (
        <>
            <Text style={styles.title}>Категорії</Text>

            <View style={styles.container}>
                <FlatList
                    data={categories}
                    keyExtractor={(item) => item.id.toString()}
                    renderItem={({item}) => (
                        <View style={styles.categoryContainer}>
                            <Image source={{uri: getImageUrl(item.image, 1200)}}
                                   style={styles.image}/>
                            <Text style={styles.name}>{item.name}</Text>
                        </View>
                    )}
                />
            </View>
        </>
    );
}

const styles = StyleSheet.create({
    title: {
        backgroundColor: '#fff',
        textAlign: 'center',
        paddingTop: 40,
        fontSize: 24,
    },
    container: {
        flex: 1,
        padding: 16,
        backgroundColor: '#fff',
    },
    categoryContainer: {
        borderRadius: 10,
        backgroundColor: '#f5f5f5',
        overflow: 'hidden',
        marginBottom: 16,
        flexDirection: 'column',
        alignItems: 'center',
    },
    image: {
        width: '100%',
        height: 200,
        objectFit: 'cover',
    },
    name: {
        fontSize: 18,
        padding: 10,
    },
});