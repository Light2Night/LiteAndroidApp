import {Button, StyleSheet, Text, TextInput, View, Image, ScrollView, SafeAreaView} from 'react-native';
import React, {useState} from "react";
import axios from "axios";
import {getActionUrl} from "@/app/apiHelper";
import IJwtTokenResponse from "@/app/interfaces/IJwtTokenResponse";
import * as ImagePicker from 'expo-image-picker';
import Spacer from "@/app/Components/Spacer";

export default function RegistrationScreen() {
    const [email, setEmail] = useState('');
    const [login, setLogin] = useState('');
    const [password, setPassword] = useState('');
    const [repeatPassword, setRepeatPassword] = useState('');
    const [firstName, setFirstName] = useState('');
    const [lastName, setLastName] = useState('');
    const [imageUri, setImageUri] = useState<string | undefined>(undefined);
    const [error, setError] = useState('');

    const handleRegistration = () => {
        // const form = new FormData();
        //
        // form.append('email', email);
        // form.append('password', password);
        //
        // axios.postForm<IJwtTokenResponse>(getActionUrl('accounts', 'signin'), form)
        //     .then(response => {
        //         setError('');
        //
        //         console.log(response.data.token);
        //     })
        //     .catch(error => {
        //         if (error.response.status === 401) {
        //             setError('Invalid email or password');
        //         } else {
        //             setError('Error');
        //         }
        //     });
    }

    const pickImage = async () => {
        const permissionResult = await ImagePicker.requestMediaLibraryPermissionsAsync();

        if (!permissionResult.granted) {
            alert('Permission to access gallery is required!')
            return
        }

        const result = await ImagePicker.launchImageLibraryAsync({
            mediaTypes: ImagePicker.MediaTypeOptions.Images,
        });

        if (!result.canceled) {
            setImageUri(result.assets[0].uri)
        }
    };

    return (
        <SafeAreaView>
            <ScrollView>
                <View style={styles.container}>
                    <Text style={styles.header}>Реєстрація</Text>
                    {error ? <Text style={styles.error}>{error}</Text> : null}

                    <Text style={styles.title}>Email</Text>
                    <TextInput
                        value={email}
                        onChangeText={setEmail}
                        autoCapitalize="none"
                        keyboardType="email-address"
                        style={styles.input}
                    />

                    <Text style={styles.title}>Логін</Text>
                    <TextInput
                        value={login}
                        onChangeText={setLogin}
                        autoCapitalize="none"
                        keyboardType="default"
                        style={styles.input}
                    />

                    <Text style={styles.title}>Пароль</Text>
                    <TextInput
                        value={password}
                        onChangeText={setPassword}
                        secureTextEntry
                        style={styles.input}
                    />

                    <Text style={styles.title}>Повторіть пароль</Text>
                    <TextInput
                        value={repeatPassword}
                        onChangeText={setRepeatPassword}
                        secureTextEntry
                        style={styles.input}
                    />

                    <Text style={styles.title}>Ім'я</Text>
                    <TextInput
                        value={firstName}
                        onChangeText={setFirstName}
                        secureTextEntry
                        style={styles.input}
                    />

                    <Text style={styles.title}>Прізвище</Text>
                    <TextInput
                        value={lastName}
                        onChangeText={setLastName}
                        secureTextEntry
                        style={styles.input}
                    />

                    {imageUri &&
                        <Image source={{uri: imageUri}} style={styles.image}/>
                    }
                    <Button title="Обрати зображення" onPress={pickImage}/>

                    <Spacer size={10}/>

                    <Button
                        title="Зареєструватися"
                        onPress={handleRegistration}
                    />
                </View>
            </ScrollView>
        </SafeAreaView>
    );
}

const styles = StyleSheet.create({
    header: {
        backgroundColor: '#fff',
        textAlign: 'center',
        paddingTop: 40,
        fontSize: 24,
    },
    container: {
        flex: 1,
        justifyContent: 'center',
        padding: 16,
        backgroundColor: '#fff',
    },
    input: {
        marginBottom: 16,
        borderWidth: 1,
        borderColor: '#ccc',
        borderRadius: 4,
        padding: 8,
    },
    error: {
        color: 'red',
        marginBottom: 16,
        textAlign: 'center',
    },
    image: {
        width: 100,
        height: 100,
        marginTop: 16,
        marginBottom: 16,
    },
    title: {
        fontSize: 16,
        paddingStart: 8
    }
});