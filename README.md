# SI_Gr15_2025 - PBKDF2-SHA512 + AES-GCM
Implementimi i PBKDF2-SHA512 në .NET për mbrojtjen e të dhënave

## Përmbledhje projekti
Ky projekt është një model i menaxhimit të fjalëkalimeve dhe kriptimit të mesazheve duke përdorur algoritmin **PBKDF2-SHA512** për derivimin e çelësave dhe **AES-GCM** për enkriptim/dekriptim. Projekti është i ndërtuar modularisht dhe mund të zgjerohet lehtësisht për të përfshirë algoritme të tjera të enkriptimit.

- **Fokus kryesor:** Siguria e ruajtjes së fjalëkalimeve dhe mesazheve.
- **Platforma:** .NET 9.0  
- **Gjuha e përdorur:** C#  
- **Arkitektura:** Modular, me mundësi zgjerimi për algoritme të tjera të enkriptimit.

---

## Struktura e projektit
SI_Gr15_2025/  
│  
├─ CryptoLib/ # Libraria kriptografike  
│ ├─ AESGCMEncryptor.cs # Implementimi konkret AES-GCM  
│ ├─ IEncryptor.cs # Ndërfaqe për enkriptim/dekriptim (për zgjerim)  
│ ├─ SecurityHelper.cs # Gjenerimi i nonce dhe byte të rastësishëm  
│ ├─ UserKeyManager.cs # Derivimi i çelësave nga fjalëkalimet  
│ └─ UserPasswordManager.cs # Menaxhimi i fjalëkalimeve (hasho & verifiko)  
│  
├─ Program.cs # Pika hyrëse (programi kryesor) dhe shembulli i përdorimit  
├─ SI_Gr15_2025.csproj # File projekti .NET  
│  
├─ .gitignore # Përjashtime për git  
├─ LICENSE # Licenca e projektit  
└─ README.md # Përshkrimi dhe udhëzimet  

## Funksionalitetet kryesore

### 1. Menaxhimi i fjalëkalimeve
- Fjalëkalimet ruhen duke përdorur **PBKDF2-SHA512** me një **salt unik** dhe numër iteracionesh të larta (250,000).
- Output-i është i ndarë dhe i lexueshëm:
    
   Algoritmi : PBKDF2-SHA512  
   Iteracionet: {iterations}  
   Salt: {saltBase64}  
   Hash: {hashBase64}  
   Fjalekalimi valid:  {True/Fasle}
    
 Sigurohet verifikim i fjalëkalimeve me **FixedTimeEquals** për të shmangur sulmet (timing attacks).

 ### 2. Derivimi i çelësave
- Çelësat simetrik derivohen nga fjalëkalimet e përdoruesve për të kryer enkriptim.  
- Moduli `UserKeyManager` siguron këtë funksionalitet me PBKDF2-SHA512.

### 3. Enkriptimi dhe dekriptimi i mesazheve
- AES në modalitetin **GCM (Galois/Counter Mode)** përdoret për enkriptimin e mesazheve.
- Përveç tekstit të enkoduar, AES-GCM gjeneron:
  - **Nonce (12 bytes)** – i përdorur vetëm një herë për çdo mesazh.  
  - **Tag (16 bytes)** – për verifikim të integritetit dhe autentikimit.
- Output-i është i ndarë dhe i lexueshëm:
    
   Nonce : {base64}  
   Ciphertext : {base64}  
   Tag : {base64}  

## Si të ekzekutoni projektin  

Kaloni në terminal dhe ekzekutoni hapat e mëtutjeshëm:  

1. **Clone repository**:
git clone https://github.com/adonisthedev/SI_Gr15_2025.git  
cd SI_Gr15_2025\SI_Gr15_2025

2. **Sigurohuni qe jeni në degën kryesore (main)**:  
git checkout main  

3. **Ndërtoni projektin**:  
dotnet build

4. **Ekzekutoni programin**:  
dotnet run

## Struktura e përdorueshme dhe e zgjerueshme  
IEncryptor lejon shtimin e algoritmeve të tjera të enkriptimit pa ndryshuar logjikën ekzistuese.  

## Licenca
Ky projekt është me licencë të MIT.
