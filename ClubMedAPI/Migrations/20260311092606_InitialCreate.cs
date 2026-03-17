using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace ClubMedAPI.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "adresse",
                columns: table => new
                {
                    numadresse = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    numrue = table.Column<int>(type: "integer", nullable: false),
                    nomrue = table.Column<string>(type: "character varying(1024)", maxLength: 1024, nullable: false),
                    codepostal = table.Column<string>(type: "character varying(5)", maxLength: 5, nullable: false),
                    ville = table.Column<string>(type: "character varying(1024)", maxLength: 1024, nullable: false),
                    pays = table.Column<string>(type: "character varying(1024)", maxLength: 1024, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_adresse", x => x.numadresse);
                });

            migrationBuilder.CreateTable(
                name: "calendrier",
                columns: table => new
                {
                    date = table.Column<DateOnly>(type: "date", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_calendrier", x => x.date);
                });

            migrationBuilder.CreateTable(
                name: "categorie",
                columns: table => new
                {
                    numcategory = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    nomcategory = table.Column<string>(type: "character varying(1024)", maxLength: 1024, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_categorie", x => x.numcategory);
                });

            migrationBuilder.CreateTable(
                name: "date_calendrier",
                columns: table => new
                {
                    jour = table.Column<DateOnly>(type: "date", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_date_calendrier", x => x.jour);
                });

            migrationBuilder.CreateTable(
                name: "equipementsalledebain",
                columns: table => new
                {
                    numequipementsallebain = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    nom = table.Column<string>(type: "character varying(1024)", maxLength: 1024, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_equipementsalledebain", x => x.numequipementsallebain);
                });

            migrationBuilder.CreateTable(
                name: "localisation",
                columns: table => new
                {
                    numlocalisation = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    nomlocalisation = table.Column<string>(type: "character varying(1024)", maxLength: 1024, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_localisation", x => x.numlocalisation);
                });

            migrationBuilder.CreateTable(
                name: "partenaires",
                columns: table => new
                {
                    idpartenaire = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    nom = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: true),
                    email = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: true),
                    telephone = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_partenaires", x => x.idpartenaire);
                });

            migrationBuilder.CreateTable(
                name: "periode",
                columns: table => new
                {
                    numperiode = table.Column<string>(type: "character(10)", maxLength: 10, nullable: false),
                    datedeb = table.Column<DateOnly>(type: "date", nullable: true),
                    datefin = table.Column<DateOnly>(type: "date", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_periode", x => x.numperiode);
                });

            migrationBuilder.CreateTable(
                name: "photo",
                columns: table => new
                {
                    numphoto = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    url = table.Column<string>(type: "character varying(1024)", maxLength: 1024, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_photo", x => x.numphoto);
                });

            migrationBuilder.CreateTable(
                name: "pointfort",
                columns: table => new
                {
                    numpointfort = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    nom = table.Column<string>(type: "character varying(1024)", maxLength: 1024, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_pointfort", x => x.numpointfort);
                });

            migrationBuilder.CreateTable(
                name: "regroupement",
                columns: table => new
                {
                    numregroupement = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    libelleregroupement = table.Column<string>(type: "character varying(1024)", maxLength: 1024, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_regroupement", x => x.numregroupement);
                });

            migrationBuilder.CreateTable(
                name: "service",
                columns: table => new
                {
                    numservice = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    nom = table.Column<string>(type: "character varying(1024)", maxLength: 1024, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_service", x => x.numservice);
                });

            migrationBuilder.CreateTable(
                name: "trancheage",
                columns: table => new
                {
                    numtranche = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    agemin = table.Column<int>(type: "integer", nullable: true),
                    agemax = table.Column<int>(type: "integer", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_trancheage", x => x.numtranche);
                });

            migrationBuilder.CreateTable(
                name: "transport",
                columns: table => new
                {
                    idtransport = table.Column<string>(type: "character(10)", maxLength: 10, nullable: false),
                    lieudepart = table.Column<string>(type: "character varying(1024)", maxLength: 1024, nullable: true),
                    prix = table.Column<decimal>(type: "numeric(10,2)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_transport", x => x.idtransport);
                });

            migrationBuilder.CreateTable(
                name: "typeclub",
                columns: table => new
                {
                    numtype = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    nomtype = table.Column<string>(type: "character varying(1024)", maxLength: 1024, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_typeclub", x => x.numtype);
                });

            migrationBuilder.CreateTable(
                name: "client",
                columns: table => new
                {
                    numclient = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    numadresse = table.Column<int>(type: "integer", nullable: true),
                    genre = table.Column<string>(type: "character varying(1024)", maxLength: 1024, nullable: true),
                    prenom = table.Column<string>(type: "character varying(1024)", maxLength: 1024, nullable: false),
                    nom = table.Column<string>(type: "character varying(1024)", maxLength: 1024, nullable: false),
                    datenaissance = table.Column<DateOnly>(type: "date", nullable: true),
                    email = table.Column<string>(type: "character varying(1024)", maxLength: 1024, nullable: false),
                    telephone = table.Column<string>(type: "character varying(1024)", maxLength: 1024, nullable: true),
                    motdepasse_crypter = table.Column<string>(type: "character varying(1024)", maxLength: 1024, nullable: false),
                    numcartebancaire_crypter = table.Column<string>(type: "character varying(1024)", maxLength: 1024, nullable: true),
                    dateexpiration_carte_bancaire = table.Column<string>(type: "character varying(5)", maxLength: 5, nullable: true),
                    cvv_crypter = table.Column<string>(type: "character varying(1024)", maxLength: 1024, nullable: true),
                    role = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false, defaultValue: "client"),
                    a2f_active = table.Column<bool>(type: "boolean", nullable: false, defaultValue: false),
                    stripe_id = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: true),
                    pm_type = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: true),
                    pm_last_four = table.Column<string>(type: "character varying(4)", maxLength: 4, nullable: true),
                    trial_ends_at = table.Column<DateTime>(type: "timestamp(0) without time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_client", x => x.numclient);
                    table.ForeignKey(
                        name: "FK_client_adresse_numadresse",
                        column: x => x.numadresse,
                        principalTable: "adresse",
                        principalColumn: "numadresse");
                });

            migrationBuilder.CreateTable(
                name: "fusionne_avec",
                columns: table => new
                {
                    numcategory = table.Column<int>(type: "integer", nullable: false),
                    numlocalisation = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_fusionne_avec", x => new { x.numcategory, x.numlocalisation });
                    table.ForeignKey(
                        name: "FK_fusionne_avec_categorie_numcategory",
                        column: x => x.numcategory,
                        principalTable: "categorie",
                        principalColumn: "numcategory",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_fusionne_avec_localisation_numlocalisation",
                        column: x => x.numlocalisation,
                        principalTable: "localisation",
                        principalColumn: "numlocalisation",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "activite",
                columns: table => new
                {
                    idactivite = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    titre = table.Column<string>(type: "character varying(1024)", maxLength: 1024, nullable: true),
                    description = table.Column<string>(type: "character varying(1024)", maxLength: 1024, nullable: false),
                    prixmin = table.Column<decimal>(type: "numeric", nullable: false),
                    idpartenaire = table.Column<int>(type: "integer", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_activite", x => x.idactivite);
                    table.ForeignKey(
                        name: "FK_activite_partenaires_idpartenaire",
                        column: x => x.idpartenaire,
                        principalTable: "partenaires",
                        principalColumn: "idpartenaire");
                });

            migrationBuilder.CreateTable(
                name: "lieurestauration",
                columns: table => new
                {
                    numrestauration = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    numphoto = table.Column<int>(type: "integer", nullable: false),
                    presentation = table.Column<string>(type: "character varying(1024)", maxLength: 1024, nullable: true),
                    nom = table.Column<string>(type: "character varying(1024)", maxLength: 1024, nullable: false),
                    description = table.Column<string>(type: "character varying(1024)", maxLength: 1024, nullable: false),
                    estbar = table.Column<bool>(type: "boolean", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_lieurestauration", x => x.numrestauration);
                    table.ForeignKey(
                        name: "FK_lieurestauration_photo_numphoto",
                        column: x => x.numphoto,
                        principalTable: "photo",
                        principalColumn: "numphoto",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "souslocalisation",
                columns: table => new
                {
                    numpays = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    numphoto = table.Column<int>(type: "integer", nullable: false),
                    nompays = table.Column<string>(type: "character varying(1024)", maxLength: 1024, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_souslocalisation", x => x.numpays);
                    table.ForeignKey(
                        name: "FK_souslocalisation_photo_numphoto",
                        column: x => x.numphoto,
                        principalTable: "photo",
                        principalColumn: "numphoto",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "station",
                columns: table => new
                {
                    numstation = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    numphoto = table.Column<int>(type: "integer", nullable: false),
                    nomstation = table.Column<string>(type: "character varying(1024)", maxLength: 1024, nullable: false),
                    altitudestation = table.Column<decimal>(type: "numeric", nullable: false),
                    longueurpistes = table.Column<decimal>(type: "numeric", nullable: false),
                    nbpistes = table.Column<int>(type: "integer", nullable: false),
                    infoski = table.Column<string>(type: "character varying(1024)", maxLength: 1024, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_station", x => x.numstation);
                    table.ForeignKey(
                        name: "FK_station_photo_numphoto",
                        column: x => x.numphoto,
                        principalTable: "photo",
                        principalColumn: "numphoto",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "typeactivite",
                columns: table => new
                {
                    numtypeactivite = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    numphoto = table.Column<int>(type: "integer", nullable: false),
                    description = table.Column<string>(type: "character varying(4096)", maxLength: 4096, nullable: false),
                    nbactivitecarte = table.Column<int>(type: "integer", nullable: false),
                    nbactiviteincluse = table.Column<int>(type: "integer", nullable: false),
                    titre = table.Column<string>(type: "character varying(1024)", maxLength: 1024, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_typeactivite", x => x.numtypeactivite);
                    table.ForeignKey(
                        name: "FK_typeactivite_photo_numphoto",
                        column: x => x.numphoto,
                        principalTable: "photo",
                        principalColumn: "numphoto",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "icon",
                columns: table => new
                {
                    numicon = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    numpointfort = table.Column<int>(type: "integer", nullable: false),
                    numservice = table.Column<int>(type: "integer", nullable: false),
                    numequipementsallebain = table.Column<int>(type: "integer", nullable: false),
                    lienicon = table.Column<string>(type: "character varying(1024)", maxLength: 1024, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_icon", x => x.numicon);
                    table.ForeignKey(
                        name: "FK_icon_equipementsalledebain_numequipementsallebain",
                        column: x => x.numequipementsallebain,
                        principalTable: "equipementsalledebain",
                        principalColumn: "numequipementsallebain",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_icon_pointfort_numpointfort",
                        column: x => x.numpointfort,
                        principalTable: "pointfort",
                        principalColumn: "numpointfort",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_icon_service_numservice",
                        column: x => x.numservice,
                        principalTable: "service",
                        principalColumn: "numservice",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "s_harmonise_avec",
                columns: table => new
                {
                    numcategory = table.Column<int>(type: "integer", nullable: false),
                    numtype = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_s_harmonise_avec", x => new { x.numcategory, x.numtype });
                    table.ForeignKey(
                        name: "FK_s_harmonise_avec_categorie_numcategory",
                        column: x => x.numcategory,
                        principalTable: "categorie",
                        principalColumn: "numcategory",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_s_harmonise_avec_typeclub_numtype",
                        column: x => x.numtype,
                        principalTable: "typeclub",
                        principalColumn: "numtype",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "carte_bancaire",
                columns: table => new
                {
                    idcb = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    numclient = table.Column<int>(type: "integer", nullable: true),
                    numcartebancaire_crypter = table.Column<string>(type: "character varying(1024)", maxLength: 1024, nullable: true),
                    dateexpiration_carte_bancaire = table.Column<string>(type: "character varying(5)", maxLength: 5, nullable: true),
                    cvv_crypter = table.Column<string>(type: "character varying(1024)", maxLength: 1024, nullable: true),
                    est_active = table.Column<bool>(type: "boolean", nullable: false, defaultValue: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_carte_bancaire", x => x.idcb);
                    table.ForeignKey(
                        name: "FK_carte_bancaire_client_numclient",
                        column: x => x.numclient,
                        principalTable: "client",
                        principalColumn: "numclient");
                });

            migrationBuilder.CreateTable(
                name: "activiteenfant",
                columns: table => new
                {
                    idactivite = table.Column<int>(type: "integer", nullable: false),
                    numtranche = table.Column<int>(type: "integer", nullable: false),
                    titre = table.Column<string>(type: "character varying(1024)", maxLength: 1024, nullable: false),
                    description = table.Column<string>(type: "character varying(1024)", maxLength: 1024, nullable: false),
                    prixmin = table.Column<decimal>(type: "numeric", nullable: false),
                    detail = table.Column<string>(type: "character varying(1024)", maxLength: 1024, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_activiteenfant", x => x.idactivite);
                    table.ForeignKey(
                        name: "FK_activiteenfant_activite_idactivite",
                        column: x => x.idactivite,
                        principalTable: "activite",
                        principalColumn: "idactivite",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_activiteenfant_trancheage_numtranche",
                        column: x => x.numtranche,
                        principalTable: "trancheage",
                        principalColumn: "numtranche",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "club",
                columns: table => new
                {
                    idclub = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    numphoto = table.Column<int>(type: "integer", nullable: false),
                    titre = table.Column<string>(type: "character varying(1024)", maxLength: 1024, nullable: true),
                    description = table.Column<string>(type: "character varying(1024)", maxLength: 1024, nullable: true),
                    notemoyenne = table.Column<decimal>(type: "numeric", nullable: true),
                    lienpdf = table.Column<string>(type: "character varying(1024)", maxLength: 1024, nullable: true),
                    numpays = table.Column<int>(type: "integer", nullable: true),
                    email = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: true),
                    statut_mise_en_ligne = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false, defaultValue: "EN_CREATION")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_club", x => x.idclub);
                    table.ForeignKey(
                        name: "FK_club_photo_numphoto",
                        column: x => x.numphoto,
                        principalTable: "photo",
                        principalColumn: "numphoto",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_club_souslocalisation_numpays",
                        column: x => x.numpays,
                        principalTable: "souslocalisation",
                        principalColumn: "numpays");
                });

            migrationBuilder.CreateTable(
                name: "s_articule_autour_de",
                columns: table => new
                {
                    numlocalisation = table.Column<int>(type: "integer", nullable: false),
                    numpays = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_s_articule_autour_de", x => new { x.numlocalisation, x.numpays });
                    table.ForeignKey(
                        name: "FK_s_articule_autour_de_localisation_numlocalisation",
                        column: x => x.numlocalisation,
                        principalTable: "localisation",
                        principalColumn: "numlocalisation",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_s_articule_autour_de_souslocalisation_numpays",
                        column: x => x.numpays,
                        principalTable: "souslocalisation",
                        principalColumn: "numpays",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "activiteadulte",
                columns: table => new
                {
                    idactivite = table.Column<int>(type: "integer", nullable: false),
                    numtypeactivite = table.Column<int>(type: "integer", nullable: false),
                    titre = table.Column<string>(type: "character varying(1024)", maxLength: 1024, nullable: true),
                    description = table.Column<string>(type: "character varying(1024)", maxLength: 1024, nullable: false),
                    prixmin = table.Column<decimal>(type: "numeric", nullable: false),
                    duree = table.Column<decimal>(type: "numeric", nullable: false),
                    ageminimum = table.Column<int>(type: "integer", nullable: false),
                    frequence = table.Column<string>(type: "character varying(1024)", maxLength: 1024, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_activiteadulte", x => x.idactivite);
                    table.ForeignKey(
                        name: "FK_activiteadulte_activite_idactivite",
                        column: x => x.idactivite,
                        principalTable: "activite",
                        principalColumn: "idactivite",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_activiteadulte_typeactivite_numtypeactivite",
                        column: x => x.numtypeactivite,
                        principalTable: "typeactivite",
                        principalColumn: "numtypeactivite",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "equipement",
                columns: table => new
                {
                    numequipement = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    numicon = table.Column<int>(type: "integer", nullable: false),
                    nom = table.Column<string>(type: "character varying(1024)", maxLength: 1024, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_equipement", x => x.numequipement);
                    table.ForeignKey(
                        name: "FK_equipement_icon_numicon",
                        column: x => x.numicon,
                        principalTable: "icon",
                        principalColumn: "numicon",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "clubstation",
                columns: table => new
                {
                    idclub = table.Column<int>(type: "integer", nullable: false),
                    numstation = table.Column<int>(type: "integer", nullable: false),
                    numphoto = table.Column<int>(type: "integer", nullable: true),
                    titre = table.Column<string>(type: "character varying(1024)", maxLength: 1024, nullable: true),
                    description = table.Column<string>(type: "character varying(1024)", maxLength: 1024, nullable: false),
                    notemoyenne = table.Column<decimal>(type: "numeric", nullable: true),
                    lienpdf = table.Column<string>(type: "character varying(1024)", maxLength: 1024, nullable: true),
                    altitudeclub = table.Column<string>(type: "character(10)", maxLength: 10, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_clubstation", x => new { x.idclub, x.numstation });
                    table.ForeignKey(
                        name: "FK_clubstation_club_idclub",
                        column: x => x.idclub,
                        principalTable: "club",
                        principalColumn: "idclub",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_clubstation_photo_numphoto",
                        column: x => x.numphoto,
                        principalTable: "photo",
                        principalColumn: "numphoto");
                    table.ForeignKey(
                        name: "FK_clubstation_station_numstation",
                        column: x => x.numstation,
                        principalTable: "station",
                        principalColumn: "numstation",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "collabore",
                columns: table => new
                {
                    idclub = table.Column<int>(type: "integer", nullable: false),
                    numcategory = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_collabore", x => new { x.idclub, x.numcategory });
                    table.ForeignKey(
                        name: "FK_collabore_categorie_numcategory",
                        column: x => x.numcategory,
                        principalTable: "categorie",
                        principalColumn: "numcategory",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_collabore_club_idclub",
                        column: x => x.idclub,
                        principalTable: "club",
                        principalColumn: "idclub",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "converge_vers",
                columns: table => new
                {
                    idclub = table.Column<int>(type: "integer", nullable: false),
                    numregroupement = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_converge_vers", x => new { x.idclub, x.numregroupement });
                    table.ForeignKey(
                        name: "FK_converge_vers_club_idclub",
                        column: x => x.idclub,
                        principalTable: "club",
                        principalColumn: "idclub",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_converge_vers_regroupement_numregroupement",
                        column: x => x.numregroupement,
                        principalTable: "regroupement",
                        principalColumn: "numregroupement",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "fusionne",
                columns: table => new
                {
                    idclub = table.Column<int>(type: "integer", nullable: false),
                    numrestauration = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_fusionne", x => new { x.idclub, x.numrestauration });
                    table.ForeignKey(
                        name: "FK_fusionne_club_idclub",
                        column: x => x.idclub,
                        principalTable: "club",
                        principalColumn: "idclub",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_fusionne_lieurestauration_numrestauration",
                        column: x => x.numrestauration,
                        principalTable: "lieurestauration",
                        principalColumn: "numrestauration",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "incruste_avec",
                columns: table => new
                {
                    idactivite = table.Column<int>(type: "integer", nullable: false),
                    idclub = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_incruste_avec", x => new { x.idactivite, x.idclub });
                    table.ForeignKey(
                        name: "FK_incruste_avec_activite_idactivite",
                        column: x => x.idactivite,
                        principalTable: "activite",
                        principalColumn: "idactivite",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_incruste_avec_club_idclub",
                        column: x => x.idclub,
                        principalTable: "club",
                        principalColumn: "idclub",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "photo_club",
                columns: table => new
                {
                    idclub = table.Column<int>(type: "integer", nullable: false),
                    numphoto = table.Column<int>(type: "integer", nullable: false),
                    ordre = table.Column<int>(type: "integer", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_photo_club", x => new { x.idclub, x.numphoto });
                    table.ForeignKey(
                        name: "FK_photo_club_club_idclub",
                        column: x => x.idclub,
                        principalTable: "club",
                        principalColumn: "idclub",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_photo_club_photo_numphoto",
                        column: x => x.numphoto,
                        principalTable: "photo",
                        principalColumn: "numphoto",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "reservation",
                columns: table => new
                {
                    numreservation = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    idclub = table.Column<int>(type: "integer", nullable: false),
                    idtransport = table.Column<string>(type: "character(10)", maxLength: 10, nullable: false),
                    numclient = table.Column<int>(type: "integer", nullable: false),
                    datedebut = table.Column<DateOnly>(type: "date", nullable: true),
                    datefin = table.Column<DateOnly>(type: "date", nullable: true),
                    nbpersonnes = table.Column<int>(type: "integer", nullable: true),
                    lieudepart = table.Column<string>(type: "character varying(1024)", maxLength: 1024, nullable: true),
                    prix = table.Column<decimal>(type: "numeric", nullable: true),
                    statut = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true, defaultValue: "EN_ATTENTE"),
                    etat_calcule = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: true),
                    mail = table.Column<bool>(type: "boolean", nullable: false, defaultValue: false),
                    disponibilite_confirmee = table.Column<bool>(type: "boolean", nullable: false, defaultValue: false),
                    token_partenaire = table.Column<string>(type: "character varying(64)", maxLength: 64, nullable: true),
                    mail_confirmation_envoye = table.Column<bool>(type: "boolean", nullable: true),
                    veut_annuler = table.Column<bool>(type: "boolean", nullable: false, defaultValue: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_reservation", x => x.numreservation);
                    table.ForeignKey(
                        name: "FK_reservation_client_numclient",
                        column: x => x.numclient,
                        principalTable: "client",
                        principalColumn: "numclient",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_reservation_club_idclub",
                        column: x => x.idclub,
                        principalTable: "club",
                        principalColumn: "idclub",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_reservation_transport_idtransport",
                        column: x => x.idtransport,
                        principalTable: "transport",
                        principalColumn: "idtransport",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "typechambre",
                columns: table => new
                {
                    idtypechambre = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    numphoto = table.Column<int>(type: "integer", nullable: false),
                    nomtype = table.Column<string>(type: "character varying(1024)", maxLength: 1024, nullable: true),
                    metrescarres = table.Column<decimal>(type: "numeric", nullable: true),
                    textepresentation = table.Column<string>(type: "character varying(1024)", maxLength: 1024, nullable: true),
                    capacitemax = table.Column<int>(type: "integer", nullable: true),
                    idclub = table.Column<int>(type: "integer", nullable: true),
                    indisponible = table.Column<bool>(type: "boolean", nullable: false, defaultValue: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_typechambre", x => x.idtypechambre);
                    table.ForeignKey(
                        name: "FK_typechambre_club_idclub",
                        column: x => x.idclub,
                        principalTable: "club",
                        principalColumn: "idclub");
                    table.ForeignKey(
                        name: "FK_typechambre_photo_numphoto",
                        column: x => x.numphoto,
                        principalTable: "photo",
                        principalColumn: "numphoto",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "autrevoyageur",
                columns: table => new
                {
                    numvoyageur = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    numreservation = table.Column<int>(type: "integer", nullable: false),
                    genre = table.Column<string>(type: "character varying(1024)", maxLength: 1024, nullable: true),
                    prenom = table.Column<string>(type: "character varying(1024)", maxLength: 1024, nullable: true),
                    nom = table.Column<string>(type: "character varying(1024)", maxLength: 1024, nullable: true),
                    datenaissance = table.Column<DateOnly>(type: "date", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_autrevoyageur", x => x.numvoyageur);
                    table.ForeignKey(
                        name: "FK_autrevoyageur_reservation_numreservation",
                        column: x => x.numreservation,
                        principalTable: "reservation",
                        principalColumn: "numreservation",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "avis",
                columns: table => new
                {
                    numavis = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    idclub = table.Column<int>(type: "integer", nullable: false),
                    numclient = table.Column<int>(type: "integer", nullable: false),
                    titre = table.Column<string>(type: "character varying(1024)", maxLength: 1024, nullable: true),
                    commentaire = table.Column<string>(type: "character varying(1024)", maxLength: 1024, nullable: false),
                    note = table.Column<int>(type: "integer", nullable: false),
                    numreservation = table.Column<int>(type: "integer", nullable: false),
                    reponse = table.Column<string>(type: "character varying(1024)", maxLength: 1024, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_avis", x => x.numavis);
                    table.ForeignKey(
                        name: "FK_avis_client_numclient",
                        column: x => x.numclient,
                        principalTable: "client",
                        principalColumn: "numclient",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_avis_club_idclub",
                        column: x => x.idclub,
                        principalTable: "club",
                        principalColumn: "idclub",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_avis_reservation_numreservation",
                        column: x => x.numreservation,
                        principalTable: "reservation",
                        principalColumn: "numreservation",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "se_lie_a",
                columns: table => new
                {
                    numreservation = table.Column<int>(type: "integer", nullable: false),
                    idactivite = table.Column<int>(type: "integer", nullable: false),
                    nbpersonnes = table.Column<int>(type: "integer", nullable: false),
                    disponibilite_confirmee = table.Column<bool>(type: "boolean", nullable: false, defaultValue: false),
                    token = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: true),
                    date_envoi = table.Column<DateOnly>(type: "date", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_se_lie_a", x => new { x.numreservation, x.idactivite });
                    table.ForeignKey(
                        name: "FK_se_lie_a_activite_idactivite",
                        column: x => x.idactivite,
                        principalTable: "activite",
                        principalColumn: "idactivite",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_se_lie_a_reservation_numreservation",
                        column: x => x.numreservation,
                        principalTable: "reservation",
                        principalColumn: "numreservation",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "transaction",
                columns: table => new
                {
                    idtransaction = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    numreservation = table.Column<int>(type: "integer", nullable: false),
                    montant = table.Column<decimal>(type: "numeric", nullable: true),
                    date_transaction = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    moyen_paiement = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    statut_paiement = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    idcb = table.Column<int>(type: "integer", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_transaction", x => x.idtransaction);
                    table.ForeignKey(
                        name: "FK_transaction_carte_bancaire_idcb",
                        column: x => x.idcb,
                        principalTable: "carte_bancaire",
                        principalColumn: "idcb");
                    table.ForeignKey(
                        name: "FK_transaction_reservation_numreservation",
                        column: x => x.numreservation,
                        principalTable: "reservation",
                        principalColumn: "numreservation",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "assure_la_liaison_avec",
                columns: table => new
                {
                    idtypechambre = table.Column<int>(type: "integer", nullable: false),
                    numequipementsallebain = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_assure_la_liaison_avec", x => new { x.idtypechambre, x.numequipementsallebain });
                    table.ForeignKey(
                        name: "FK_assure_la_liaison_avec_equipementsalledebain_numequipements~",
                        column: x => x.numequipementsallebain,
                        principalTable: "equipementsalledebain",
                        principalColumn: "numequipementsallebain",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_assure_la_liaison_avec_typechambre_idtypechambre",
                        column: x => x.idtypechambre,
                        principalTable: "typechambre",
                        principalColumn: "idtypechambre",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "chambre",
                columns: table => new
                {
                    numchambre = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    idtypechambre = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_chambre", x => x.numchambre);
                    table.ForeignKey(
                        name: "FK_chambre_typechambre_idtypechambre",
                        column: x => x.idtypechambre,
                        principalTable: "typechambre",
                        principalColumn: "idtypechambre",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "prix_periode",
                columns: table => new
                {
                    numperiode = table.Column<string>(type: "character(10)", maxLength: 10, nullable: false),
                    idtypechambre = table.Column<int>(type: "integer", nullable: false),
                    prixperiode = table.Column<decimal>(type: "numeric", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_prix_periode", x => new { x.numperiode, x.idtypechambre });
                    table.ForeignKey(
                        name: "FK_prix_periode_periode_numperiode",
                        column: x => x.numperiode,
                        principalTable: "periode",
                        principalColumn: "numperiode",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_prix_periode_typechambre_idtypechambre",
                        column: x => x.idtypechambre,
                        principalTable: "typechambre",
                        principalColumn: "idtypechambre",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "s_imbrique_dans",
                columns: table => new
                {
                    idtypechambre = table.Column<int>(type: "integer", nullable: false),
                    numservice = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_s_imbrique_dans", x => new { x.idtypechambre, x.numservice });
                    table.ForeignKey(
                        name: "FK_s_imbrique_dans_service_numservice",
                        column: x => x.numservice,
                        principalTable: "service",
                        principalColumn: "numservice",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_s_imbrique_dans_typechambre_idtypechambre",
                        column: x => x.idtypechambre,
                        principalTable: "typechambre",
                        principalColumn: "idtypechambre",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "se_met_en_harmonie_avec",
                columns: table => new
                {
                    idtypechambre = table.Column<int>(type: "integer", nullable: false),
                    numequipement = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_se_met_en_harmonie_avec", x => new { x.idtypechambre, x.numequipement });
                    table.ForeignKey(
                        name: "FK_se_met_en_harmonie_avec_equipement_numequipement",
                        column: x => x.numequipement,
                        principalTable: "equipement",
                        principalColumn: "numequipement",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_se_met_en_harmonie_avec_typechambre_idtypechambre",
                        column: x => x.idtypechambre,
                        principalTable: "typechambre",
                        principalColumn: "idtypechambre",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "synchronise",
                columns: table => new
                {
                    idtypechambre = table.Column<int>(type: "integer", nullable: false),
                    numpointfort = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_synchronise", x => new { x.idtypechambre, x.numpointfort });
                    table.ForeignKey(
                        name: "FK_synchronise_pointfort_numpointfort",
                        column: x => x.numpointfort,
                        principalTable: "pointfort",
                        principalColumn: "numpointfort",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_synchronise_typechambre_idtypechambre",
                        column: x => x.idtypechambre,
                        principalTable: "typechambre",
                        principalColumn: "idtypechambre",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "photoavis",
                columns: table => new
                {
                    numavis = table.Column<int>(type: "integer", nullable: false),
                    numphoto = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_photoavis", x => new { x.numavis, x.numphoto });
                    table.ForeignKey(
                        name: "FK_photoavis_avis_numavis",
                        column: x => x.numavis,
                        principalTable: "avis",
                        principalColumn: "numavis",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_photoavis_photo_numphoto",
                        column: x => x.numphoto,
                        principalTable: "photo",
                        principalColumn: "numphoto",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "disponibilite",
                columns: table => new
                {
                    date = table.Column<DateOnly>(type: "date", nullable: false),
                    numchambre = table.Column<int>(type: "integer", nullable: false),
                    idclub = table.Column<int>(type: "integer", nullable: false),
                    estdisponibilite = table.Column<bool>(type: "boolean", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_disponibilite", x => new { x.date, x.numchambre, x.idclub });
                    table.ForeignKey(
                        name: "FK_disponibilite_chambre_numchambre",
                        column: x => x.numchambre,
                        principalTable: "chambre",
                        principalColumn: "numchambre",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_disponibilite_club_idclub",
                        column: x => x.idclub,
                        principalTable: "club",
                        principalColumn: "idclub",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "s_influence_mutuellement",
                columns: table => new
                {
                    idtypechambre = table.Column<int>(type: "integer", nullable: false),
                    numchambre = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_s_influence_mutuellement", x => new { x.idtypechambre, x.numchambre });
                    table.ForeignKey(
                        name: "FK_s_influence_mutuellement_chambre_numchambre",
                        column: x => x.numchambre,
                        principalTable: "chambre",
                        principalColumn: "numchambre",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_s_influence_mutuellement_typechambre_idtypechambre",
                        column: x => x.idtypechambre,
                        principalTable: "typechambre",
                        principalColumn: "idtypechambre",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "s_unit_a",
                columns: table => new
                {
                    idclub = table.Column<int>(type: "integer", nullable: false),
                    numchambre = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_s_unit_a", x => new { x.idclub, x.numchambre });
                    table.ForeignKey(
                        name: "FK_s_unit_a_chambre_numchambre",
                        column: x => x.numchambre,
                        principalTable: "chambre",
                        principalColumn: "numchambre",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_s_unit_a_club_idclub",
                        column: x => x.idclub,
                        principalTable: "club",
                        principalColumn: "idclub",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_activite_idpartenaire",
                table: "activite",
                column: "idpartenaire");

            migrationBuilder.CreateIndex(
                name: "IX_activiteadulte_numtypeactivite",
                table: "activiteadulte",
                column: "numtypeactivite");

            migrationBuilder.CreateIndex(
                name: "IX_activiteenfant_numtranche",
                table: "activiteenfant",
                column: "numtranche");

            migrationBuilder.CreateIndex(
                name: "IX_assure_la_liaison_avec_numequipementsallebain",
                table: "assure_la_liaison_avec",
                column: "numequipementsallebain");

            migrationBuilder.CreateIndex(
                name: "IX_autrevoyageur_numreservation",
                table: "autrevoyageur",
                column: "numreservation");

            migrationBuilder.CreateIndex(
                name: "IX_avis_idclub",
                table: "avis",
                column: "idclub");

            migrationBuilder.CreateIndex(
                name: "IX_avis_numclient",
                table: "avis",
                column: "numclient");

            migrationBuilder.CreateIndex(
                name: "IX_avis_numreservation",
                table: "avis",
                column: "numreservation");

            migrationBuilder.CreateIndex(
                name: "IX_carte_bancaire_numclient",
                table: "carte_bancaire",
                column: "numclient");

            migrationBuilder.CreateIndex(
                name: "IX_chambre_idtypechambre",
                table: "chambre",
                column: "idtypechambre");

            migrationBuilder.CreateIndex(
                name: "IX_client_email",
                table: "client",
                column: "email",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_client_numadresse",
                table: "client",
                column: "numadresse");

            migrationBuilder.CreateIndex(
                name: "IX_club_numpays",
                table: "club",
                column: "numpays");

            migrationBuilder.CreateIndex(
                name: "IX_club_numphoto",
                table: "club",
                column: "numphoto");

            migrationBuilder.CreateIndex(
                name: "IX_clubstation_numphoto",
                table: "clubstation",
                column: "numphoto");

            migrationBuilder.CreateIndex(
                name: "IX_clubstation_numstation",
                table: "clubstation",
                column: "numstation");

            migrationBuilder.CreateIndex(
                name: "IX_collabore_numcategory",
                table: "collabore",
                column: "numcategory");

            migrationBuilder.CreateIndex(
                name: "IX_converge_vers_numregroupement",
                table: "converge_vers",
                column: "numregroupement");

            migrationBuilder.CreateIndex(
                name: "IX_disponibilite_idclub",
                table: "disponibilite",
                column: "idclub");

            migrationBuilder.CreateIndex(
                name: "IX_disponibilite_numchambre",
                table: "disponibilite",
                column: "numchambre");

            migrationBuilder.CreateIndex(
                name: "IX_equipement_numicon",
                table: "equipement",
                column: "numicon");

            migrationBuilder.CreateIndex(
                name: "IX_fusionne_numrestauration",
                table: "fusionne",
                column: "numrestauration");

            migrationBuilder.CreateIndex(
                name: "IX_fusionne_avec_numlocalisation",
                table: "fusionne_avec",
                column: "numlocalisation");

            migrationBuilder.CreateIndex(
                name: "IX_icon_numequipementsallebain",
                table: "icon",
                column: "numequipementsallebain");

            migrationBuilder.CreateIndex(
                name: "IX_icon_numpointfort",
                table: "icon",
                column: "numpointfort");

            migrationBuilder.CreateIndex(
                name: "IX_icon_numservice",
                table: "icon",
                column: "numservice");

            migrationBuilder.CreateIndex(
                name: "IX_incruste_avec_idclub",
                table: "incruste_avec",
                column: "idclub");

            migrationBuilder.CreateIndex(
                name: "IX_lieurestauration_numphoto",
                table: "lieurestauration",
                column: "numphoto");

            migrationBuilder.CreateIndex(
                name: "IX_photo_club_numphoto",
                table: "photo_club",
                column: "numphoto");

            migrationBuilder.CreateIndex(
                name: "IX_photoavis_numphoto",
                table: "photoavis",
                column: "numphoto");

            migrationBuilder.CreateIndex(
                name: "IX_prix_periode_idtypechambre",
                table: "prix_periode",
                column: "idtypechambre");

            migrationBuilder.CreateIndex(
                name: "IX_reservation_idclub",
                table: "reservation",
                column: "idclub");

            migrationBuilder.CreateIndex(
                name: "IX_reservation_idtransport",
                table: "reservation",
                column: "idtransport");

            migrationBuilder.CreateIndex(
                name: "IX_reservation_numclient",
                table: "reservation",
                column: "numclient");

            migrationBuilder.CreateIndex(
                name: "IX_s_articule_autour_de_numpays",
                table: "s_articule_autour_de",
                column: "numpays");

            migrationBuilder.CreateIndex(
                name: "IX_s_harmonise_avec_numtype",
                table: "s_harmonise_avec",
                column: "numtype");

            migrationBuilder.CreateIndex(
                name: "IX_s_imbrique_dans_numservice",
                table: "s_imbrique_dans",
                column: "numservice");

            migrationBuilder.CreateIndex(
                name: "IX_s_influence_mutuellement_numchambre",
                table: "s_influence_mutuellement",
                column: "numchambre");

            migrationBuilder.CreateIndex(
                name: "IX_s_unit_a_numchambre",
                table: "s_unit_a",
                column: "numchambre");

            migrationBuilder.CreateIndex(
                name: "IX_se_lie_a_idactivite",
                table: "se_lie_a",
                column: "idactivite");

            migrationBuilder.CreateIndex(
                name: "IX_se_met_en_harmonie_avec_numequipement",
                table: "se_met_en_harmonie_avec",
                column: "numequipement");

            migrationBuilder.CreateIndex(
                name: "IX_souslocalisation_numphoto",
                table: "souslocalisation",
                column: "numphoto");

            migrationBuilder.CreateIndex(
                name: "IX_station_numphoto",
                table: "station",
                column: "numphoto");

            migrationBuilder.CreateIndex(
                name: "IX_synchronise_numpointfort",
                table: "synchronise",
                column: "numpointfort");

            migrationBuilder.CreateIndex(
                name: "IX_transaction_idcb",
                table: "transaction",
                column: "idcb");

            migrationBuilder.CreateIndex(
                name: "IX_transaction_numreservation",
                table: "transaction",
                column: "numreservation");

            migrationBuilder.CreateIndex(
                name: "IX_typeactivite_numphoto",
                table: "typeactivite",
                column: "numphoto");

            migrationBuilder.CreateIndex(
                name: "IX_typechambre_idclub",
                table: "typechambre",
                column: "idclub");

            migrationBuilder.CreateIndex(
                name: "IX_typechambre_numphoto",
                table: "typechambre",
                column: "numphoto");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "activiteadulte");

            migrationBuilder.DropTable(
                name: "activiteenfant");

            migrationBuilder.DropTable(
                name: "assure_la_liaison_avec");

            migrationBuilder.DropTable(
                name: "autrevoyageur");

            migrationBuilder.DropTable(
                name: "calendrier");

            migrationBuilder.DropTable(
                name: "clubstation");

            migrationBuilder.DropTable(
                name: "collabore");

            migrationBuilder.DropTable(
                name: "converge_vers");

            migrationBuilder.DropTable(
                name: "date_calendrier");

            migrationBuilder.DropTable(
                name: "disponibilite");

            migrationBuilder.DropTable(
                name: "fusionne");

            migrationBuilder.DropTable(
                name: "fusionne_avec");

            migrationBuilder.DropTable(
                name: "incruste_avec");

            migrationBuilder.DropTable(
                name: "photo_club");

            migrationBuilder.DropTable(
                name: "photoavis");

            migrationBuilder.DropTable(
                name: "prix_periode");

            migrationBuilder.DropTable(
                name: "s_articule_autour_de");

            migrationBuilder.DropTable(
                name: "s_harmonise_avec");

            migrationBuilder.DropTable(
                name: "s_imbrique_dans");

            migrationBuilder.DropTable(
                name: "s_influence_mutuellement");

            migrationBuilder.DropTable(
                name: "s_unit_a");

            migrationBuilder.DropTable(
                name: "se_lie_a");

            migrationBuilder.DropTable(
                name: "se_met_en_harmonie_avec");

            migrationBuilder.DropTable(
                name: "synchronise");

            migrationBuilder.DropTable(
                name: "transaction");

            migrationBuilder.DropTable(
                name: "typeactivite");

            migrationBuilder.DropTable(
                name: "trancheage");

            migrationBuilder.DropTable(
                name: "station");

            migrationBuilder.DropTable(
                name: "regroupement");

            migrationBuilder.DropTable(
                name: "lieurestauration");

            migrationBuilder.DropTable(
                name: "avis");

            migrationBuilder.DropTable(
                name: "periode");

            migrationBuilder.DropTable(
                name: "localisation");

            migrationBuilder.DropTable(
                name: "categorie");

            migrationBuilder.DropTable(
                name: "typeclub");

            migrationBuilder.DropTable(
                name: "chambre");

            migrationBuilder.DropTable(
                name: "activite");

            migrationBuilder.DropTable(
                name: "equipement");

            migrationBuilder.DropTable(
                name: "carte_bancaire");

            migrationBuilder.DropTable(
                name: "reservation");

            migrationBuilder.DropTable(
                name: "typechambre");

            migrationBuilder.DropTable(
                name: "partenaires");

            migrationBuilder.DropTable(
                name: "icon");

            migrationBuilder.DropTable(
                name: "client");

            migrationBuilder.DropTable(
                name: "transport");

            migrationBuilder.DropTable(
                name: "club");

            migrationBuilder.DropTable(
                name: "equipementsalledebain");

            migrationBuilder.DropTable(
                name: "pointfort");

            migrationBuilder.DropTable(
                name: "service");

            migrationBuilder.DropTable(
                name: "adresse");

            migrationBuilder.DropTable(
                name: "souslocalisation");

            migrationBuilder.DropTable(
                name: "photo");
        }
    }
}
