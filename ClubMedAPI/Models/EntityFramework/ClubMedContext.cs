using Microsoft.EntityFrameworkCore;

namespace ClubMedAPI.Models.EntityFramework
{
    public partial class ClubMedContext : DbContext
    {
        public ClubMedContext() { }
        public ClubMedContext(DbContextOptions<ClubMedContext> options) : base(options) { }

        // ===== DbSets (1 par entité) =====
        public virtual DbSet<Photo> Photos { get; set; }
        public virtual DbSet<Localisation> Localisations { get; set; }
        public virtual DbSet<SousLocalisation> SousLocalisations { get; set; }
        public virtual DbSet<Categorie> Categories { get; set; }
        public virtual DbSet<Regroupement> Regroupements { get; set; }
        public virtual DbSet<Adresse> Adresses { get; set; }
        public virtual DbSet<Client> Clients { get; set; }
        public virtual DbSet<CarteBancaire> CartesBancaires { get; set; }
        public virtual DbSet<Club> Clubs { get; set; }
        public virtual DbSet<Station> Stations { get; set; }
        public virtual DbSet<ClubStation> ClubStations { get; set; }
        public virtual DbSet<LieuRestauration> LieuxRestauration { get; set; }
        public virtual DbSet<TypeActivite> TypeActivites { get; set; }
        public virtual DbSet<TrancheAge> TrancheAges { get; set; }
        public virtual DbSet<Activite> Activites { get; set; }
        public virtual DbSet<ActiviteAdulte> ActivitesAdultes { get; set; }
        public virtual DbSet<ActiviteEnfant> ActivitesEnfants { get; set; }
        public virtual DbSet<TypeChambre> TypeChambres { get; set; }
        public virtual DbSet<Chambre> Chambres { get; set; }
        public virtual DbSet<Periode> Periodes { get; set; }
        public virtual DbSet<PrixPeriode> PrixPeriodes { get; set; }
        public virtual DbSet<Transport> Transports { get; set; }
        public virtual DbSet<Partenaire> Partenaires { get; set; }
        public virtual DbSet<Reservation> Reservations { get; set; }
        public virtual DbSet<ReservationActivite> ReservationActivites { get; set; }
        public virtual DbSet<Avis> Avis { get; set; }
        public virtual DbSet<AutreVoyageur> AutresVoyageurs { get; set; }
        public virtual DbSet<Transaction> Transactions { get; set; }
        public virtual DbSet<PointFort> PointForts { get; set; }
        public virtual DbSet<Service> Services { get; set; }
        public virtual DbSet<Equipement> Equipements { get; set; }
        public virtual DbSet<EquipementSalleDeBain> EquipementsSalleDeBain { get; set; }
        public virtual DbSet<Icon> Icons { get; set; }
        public virtual DbSet<TypeClub> TypeClubs { get; set; }
        public virtual DbSet<Calendrier> Calendriers { get; set; }
        public virtual DbSet<DateCalendrier> DateCalendriers { get; set; }
        public virtual DbSet<Disponibilite> Disponibilites { get; set; }
        public virtual DbSet<PhotoClub> PhotoClubs { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // =================================================================
            // 1. CLÉS COMPOSITES (Tables de liaison avec données pivot)
            //    Comme indiqué dans la Bible : pas de [Key] dans le modèle,
            //    on définit la clé composite ici avec Fluent API.
            // =================================================================

            modelBuilder.Entity<PrixPeriode>()
                .HasKey(pp => new { pp.NumPeriode, pp.IdTypeChambre });

            modelBuilder.Entity<ReservationActivite>()
                .HasKey(ra => new { ra.NumReservation, ra.IdActivite });

            modelBuilder.Entity<ClubStation>()
                .HasKey(cs => new { cs.IdClub, cs.NumStation });

            // Clé composite pour Disponibilite
            modelBuilder.Entity<Disponibilite>()
                .HasKey(d => new { d.Date, d.NumChambre, d.IdClub });

            modelBuilder.Entity<PhotoClub>()
                .HasKey(pc => new { pc.IdClub, pc.NumPhoto });

            // =================================================================
            // 2. MANY-TO-MANY (Tables de liaison simples sans données pivot)
            //    EF Core gère la table de liaison automatiquement.
            // =================================================================

            // Club <-> Categorie via "collabore"
            modelBuilder.Entity<Club>()
                .HasMany(c => c.Categories)
                .WithMany(cat => cat.Clubs)
                .UsingEntity(
                    "collabore",
                    l => l.HasOne(typeof(Categorie)).WithMany().HasForeignKey("numcategory"),
                    r => r.HasOne(typeof(Club)).WithMany().HasForeignKey("idclub")
                );

            // Club <-> Regroupement via "converge_vers"
            modelBuilder.Entity<Club>()
                .HasMany(c => c.Regroupements)
                .WithMany(r => r.Clubs)
                .UsingEntity(
                    "converge_vers",
                    l => l.HasOne(typeof(Regroupement)).WithMany().HasForeignKey("numregroupement"),
                    r => r.HasOne(typeof(Club)).WithMany().HasForeignKey("idclub")
                );

            // Club <-> Activite via "incruste_avec"
            modelBuilder.Entity<Club>()
                .HasMany(c => c.Activites)
                .WithMany(a => a.Clubs)
                .UsingEntity(
                    "incruste_avec",
                    l => l.HasOne(typeof(Activite)).WithMany().HasForeignKey("idactivite"),
                    r => r.HasOne(typeof(Club)).WithMany().HasForeignKey("idclub")
                );

            // Club <-> Chambre via "s_unit_a"
            modelBuilder.Entity<Club>()
                .HasMany(c => c.Chambres)
                .WithMany(ch => ch.Clubs)
                .UsingEntity(
                    "s_unit_a",
                    l => l.HasOne(typeof(Chambre)).WithMany().HasForeignKey("numchambre"),
                    r => r.HasOne(typeof(Club)).WithMany().HasForeignKey("idclub")
                );

            // Club <-> LieuRestauration via "fusionne"
            modelBuilder.Entity<Club>()
                .HasMany(c => c.LieuxRestauration)
                .WithMany(lr => lr.Clubs)
                .UsingEntity(
                    "fusionne",
                    l => l.HasOne(typeof(LieuRestauration)).WithMany().HasForeignKey("numrestauration"),
                    r => r.HasOne(typeof(Club)).WithMany().HasForeignKey("idclub")
                );

            // Club <-> Photo (galerie) via PhotoClub (table de liaison AVEC donnée pivot "ordre")
            modelBuilder.Entity<Club>()
                .HasMany(c => c.PhotosGalerie)
                .WithMany(p => p.ClubsGalerie)
                .UsingEntity<PhotoClub>(
                    l => l.HasOne(pc => pc.Photo).WithMany().HasForeignKey(pc => pc.NumPhoto),
                    r => r.HasOne(pc => pc.Club).WithMany().HasForeignKey(pc => pc.IdClub),
                    j => j.ToTable("photo_club")
                );

            // Club <-> Station via ClubStation (table de liaison AVEC données pivot)
            modelBuilder.Entity<Club>()
                .HasMany(c => c.Stations)
                .WithMany(s => s.Clubs)
                .UsingEntity<ClubStation>(
                    l => l.HasOne(cs => cs.Station).WithMany().HasForeignKey(cs => cs.NumStation),
                    r => r.HasOne(cs => cs.Club).WithMany().HasForeignKey(cs => cs.IdClub),
                    j => j.ToTable("clubstation")
                );

            // Localisation <-> SousLocalisation via "s_articule_autour_de"
            modelBuilder.Entity<Localisation>()
                .HasMany(l => l.SousLocalisations)
                .WithMany(sl => sl.Localisations)
                .UsingEntity(
                    "s_articule_autour_de",
                    l => l.HasOne(typeof(SousLocalisation)).WithMany().HasForeignKey("numpays"),
                    r => r.HasOne(typeof(Localisation)).WithMany().HasForeignKey("numlocalisation")
                );

            // Avis <-> Photo via "photoavis"
            modelBuilder.Entity<Avis>()
                .HasMany(a => a.Photos)
                .WithMany(p => p.Avis)
                .UsingEntity(
                    "photoavis",
                    l => l.HasOne(typeof(Photo)).WithMany().HasForeignKey("numphoto"),
                    r => r.HasOne(typeof(Avis)).WithMany().HasForeignKey("numavis")
                );

            // Categorie <-> Localisation via "fusionne_avec"
            modelBuilder.Entity<Categorie>()
                .HasMany(c => c.Localisations)
                .WithMany(l => l.Categories)
                .UsingEntity(
                    "fusionne_avec",
                    l => l.HasOne(typeof(Localisation)).WithMany().HasForeignKey("numlocalisation"),
                    r => r.HasOne(typeof(Categorie)).WithMany().HasForeignKey("numcategory")
                );

            // Categorie <-> TypeClub via "s_harmonise_avec"
            modelBuilder.Entity<Categorie>()
                .HasMany(c => c.TypeClubs)
                .WithMany(tc => tc.Categories)
                .UsingEntity(
                    "s_harmonise_avec",
                    l => l.HasOne(typeof(TypeClub)).WithMany().HasForeignKey("numtype"),
                    r => r.HasOne(typeof(Categorie)).WithMany().HasForeignKey("numcategory")
                );

            // TypeChambre <-> PointFort via "synchronise"
            modelBuilder.Entity<TypeChambre>()
                .HasMany(tc => tc.PointForts)
                .WithMany(pf => pf.TypeChambres)
                .UsingEntity(
                    "synchronise",
                    l => l.HasOne(typeof(PointFort)).WithMany().HasForeignKey("numpointfort"),
                    r => r.HasOne(typeof(TypeChambre)).WithMany().HasForeignKey("idtypechambre")
                );

            // TypeChambre <-> Service via "s_imbrique_dans"
            modelBuilder.Entity<TypeChambre>()
                .HasMany(tc => tc.Services)
                .WithMany(s => s.TypeChambres)
                .UsingEntity(
                    "s_imbrique_dans",
                    l => l.HasOne(typeof(Service)).WithMany().HasForeignKey("numservice"),
                    r => r.HasOne(typeof(TypeChambre)).WithMany().HasForeignKey("idtypechambre")
                );

            // TypeChambre <-> Equipement via "se_met_en_harmonie_avec"
            modelBuilder.Entity<TypeChambre>()
                .HasMany(tc => tc.Equipements)
                .WithMany(e => e.TypeChambres)
                .UsingEntity(
                    "se_met_en_harmonie_avec",
                    l => l.HasOne(typeof(Equipement)).WithMany().HasForeignKey("numequipement"),
                    r => r.HasOne(typeof(TypeChambre)).WithMany().HasForeignKey("idtypechambre")
                );

            // TypeChambre <-> EquipementSalleDeBain via "assure_la_liaison_avec"
            modelBuilder.Entity<TypeChambre>()
                .HasMany(tc => tc.EquipementsSalleDeBain)
                .WithMany(esb => esb.TypeChambres)
                .UsingEntity(
                    "assure_la_liaison_avec",
                    l => l.HasOne(typeof(EquipementSalleDeBain)).WithMany().HasForeignKey("numequipementsallebain"),
                    r => r.HasOne(typeof(TypeChambre)).WithMany().HasForeignKey("idtypechambre")
                );

            // TypeChambre <-> Chambre via "s_influence_mutuellement"
            modelBuilder.Entity<TypeChambre>()
                .HasMany(tc => tc.ChambresInfluence)
                .WithMany(c => c.TypeChambresInfluence)
                .UsingEntity(
                    "s_influence_mutuellement",
                    l => l.HasOne(typeof(Chambre)).WithMany().HasForeignKey("numchambre"),
                    r => r.HasOne(typeof(TypeChambre)).WithMany().HasForeignKey("idtypechambre")
                );

            // =================================================================
            // 3. CONTRAINTES UNIQUES
            // =================================================================

            modelBuilder.Entity<Client>()
                .HasIndex(c => c.Email)
                .IsUnique();

            // =================================================================
            // 4. VALEURS PAR DÉFAUT
            // =================================================================

            modelBuilder.Entity<Club>()
                .Property(c => c.StatutMiseEnLigne)
                .HasDefaultValue("EN_CREATION");

            modelBuilder.Entity<Client>()
                .Property(c => c.Role)
                .HasDefaultValue("client");

            modelBuilder.Entity<Client>()
                .Property(c => c.A2fActive)
                .HasDefaultValue(false);

            modelBuilder.Entity<Reservation>()
                .Property(r => r.Statut)
                .HasDefaultValue("EN_ATTENTE");

            modelBuilder.Entity<Reservation>()
                .Property(r => r.DisponibiliteConfirmee)
                .HasDefaultValue(false);

            modelBuilder.Entity<Reservation>()
                .Property(r => r.Mail)
                .HasDefaultValue(false);

            modelBuilder.Entity<Reservation>()
                .Property(r => r.VeutAnnuler)
                .HasDefaultValue(false);

            modelBuilder.Entity<ReservationActivite>()
                .Property(ra => ra.DisponibiliteConfirmee)
                .HasDefaultValue(false);

            modelBuilder.Entity<TypeChambre>()
                .Property(tc => tc.Indisponible)
                .HasDefaultValue(false);

            modelBuilder.Entity<CarteBancaire>()
                .Property(cb => cb.EstActive)
                .HasDefaultValue(true);

            // =================================================================
            // 5. COMPORTEMENT DE SUPPRESSION (ON DELETE)
            // =================================================================

            // Empêcher la suppression en cascade d'un client s'il a des réservations
            modelBuilder.Entity<Reservation>()
                .HasOne(r => r.Client)
                .WithMany(c => c.Reservations)
                .HasForeignKey(r => r.NumClient)
                .OnDelete(DeleteBehavior.Restrict);

            // Empêcher la suppression d'un club s'il a des réservations
            modelBuilder.Entity<Reservation>()
                .HasOne(r => r.Club)
                .WithMany(c => c.Reservations)
                .HasForeignKey(r => r.IdClub)
                .OnDelete(DeleteBehavior.Restrict);

            // Empêcher la suppression d'un club s'il a des avis
            modelBuilder.Entity<Avis>()
                .HasOne(a => a.Club)
                .WithMany(c => c.Avis)
                .HasForeignKey(a => a.IdClub)
                .OnDelete(DeleteBehavior.Restrict);

            // Empêcher la suppression d'une réservation si elle a des voyageurs
            modelBuilder.Entity<AutreVoyageur>()
                .HasOne(av => av.Reservation)
                .WithMany(r => r.AutresVoyageurs)
                .HasForeignKey(av => av.NumReservation)
                .OnDelete(DeleteBehavior.Cascade);

            // Transport sur une réservation
            modelBuilder.Entity<Reservation>()
                .HasOne(r => r.Transport)
                .WithMany(t => t.Reservations)
                .HasForeignKey(r => r.IdTransport)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
